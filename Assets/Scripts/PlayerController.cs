using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    private static bool _fall = false;
    private static bool _finish = false;
    private static float _power;
    private static int _agentNumber;
    public static int AgentNumber{get => _agentNumber; set => _agentNumber = value;}
    public static float AttractionRange { get => _power; set => _power = value; }
    public static bool IsFall { get => _fall; set => _fall = value; }
    public static bool IsFinish { get => _finish; set => _finish = value; }
    [SerializeField] private float _firstLine;
    [SerializeField] private float _secondLine;
    [SerializeField] private float _thirdLine;

    [SerializeField] private float _moveThreshold;
    [SerializeField] private float _speed;
    [SerializeField] private float _moveSpeed;

    [SerializeField] private Animator PlayerAnimator;
    private float _lastMoveTime;
    [SerializeField] private Rigidbody _rigidbody;
    private int _point;
    private bool _running;
    private Vector3 moveTo;
    [SerializeField] private GameObject _enemy,_player,starEfect,atractionArea,confeti,fullPowerFire;
    [SerializeField] private Transform _leftUp,_rightUp,_leftDown,_rightDown,_leftDownFoot,_rightDownFoot;
    private float _progress,_health;
    [SerializeField] private  Transform _finishLine,_character;
    [SerializeField] private float _yJump, _zJump, _time,_jumpForce;
    [SerializeField] private ParticleSystem fireParticle;
    enum Lane
    {
        First,
        Second,
        Third
    }

    private Lane _lane = Lane.Second;

	

    private void Awake()
    {   
        _rigidbody = GetComponent<Rigidbody>();
        _enemy = GetComponent<GameObject>();
        
        _running = false;
        
    }

    private void Update()
    {
        
        switch (GameManager.manager.CurrentGameState)
        {
            case GameManager.GameState.Prepare:
                print("prepare");
                Prepare();
                _finish = false;
                _health = 0f;
                _power = 0;
                
                break;
            case GameManager.GameState.MainGame:
                print("main");
                Swipe();
                //flip();
                Move(moveTo);
                _finish = false;
                //_running = true;
                
                break;
            case GameManager.GameState.FinishGame:
                print("finish");
                break;
        }
      
        
        
    }
    private void FixedUpdate()
    {
        if (_running)
        {
            _rigidbody.velocity = transform.forward * (Time.deltaTime * _moveSpeed);
            
        }

        ScoreUpdate();
        Debug.Log("Agents:" + _agentNumber);
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        _point++;
        print(_point);
        if (other.tag == "woodAnim")
        {
            if(_power<1f)
            {
                PlayerAnimator.SetTrigger("isPunch");
                StartCoroutine(PunchWood());
                Debug.Log("wood");
                
            }
            else 
            {
                
                PlayerAnimator.SetTrigger("isHeadButt");
                StartCoroutine(PunchWood());
            }
            
            
           
            
            
        }
        else if (other.tag == "Wall")
        {
            if(_power<1f)
            {PlayerAnimator.SetTrigger("isPunch");
                StartCoroutine(PunchWall());
                
            }
            // else if(_health<1f)
            //{
                
                // PlayerAnimator.SetTrigger("isFalling");
                //GameManager.manager.ToFinishGame();
                //UIManager.manager.RetryMethod();
                // _running = false;
                //_player.tag = "Untagged";//
                //_fall = true;//

           // }
            else if(_power>=1f)
            {   
                PlayerAnimator.SetTrigger("isHeadButt");
                StartCoroutine(PunchWood());
            }
            
            
            
            
        }
        else if(other.tag == "Metal")
        {
            if(_power>=1f)
            {
                
                PlayerAnimator.SetTrigger("isHeadButt");
                StartCoroutine(PunchMetal());
                
            }
            else
            {
                
                PlayerAnimator.SetTrigger("isFalling");
               // GameManager.manager.ToFinishGame();
               // UIManager.manager.RetryMethod();
                _running = false;
                _player.tag = "Untagged";
                StartCoroutine(Fall());
                //_fall = true;

            }
            
            
        }
        else if (other.tag == "Before")
        {
            moveTo = new Vector3(_secondLine, 0, transform.position.z);
            _lane = Lane.Second;
        }
        else if (other.tag == "Finish")
        {  
            _running = false;
            PlayerAnimator.SetTrigger("isFinish");
            _finish = true;
            confeti.SetActive(true);
            GameManager.manager.ToFinishGame();
            UIManager.manager.LoadNextLevel();
            
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            print("collisionEnter");
            
            _running = false;
            
            
            StartCoroutine(Fall());
            
           // _point--;
           // print(_point);
           
        }
        //else if (other.gameObject.tag == "wood")
       // {
       //     PlayerAnimator.SetTrigger("isPunch");
       //     StartCoroutine(Punch());
       //     Debug.Log("wood");
       // }
    }

    private void Move(Vector3 moveTo)
    {
        //PlayerAnimator.SetTrigger("isRunning");
        moveTo = new Vector3(moveTo.x,0,transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, moveTo, Time.deltaTime * _speed);
        _progress = _character.position.z / _finishLine.position.z;
        UIManager.manager.ProgressBar(_progress);
       
        
        _running = true;
       

    }
    
    
    private void flip()
    {
        if (Input.touchCount > 0)
        {
            
            
            Touch touch = Input.touches[0];
            float movePow = touch.deltaPosition.normalized.y;
            if (Mathf.Abs(movePow) > _moveThreshold && Time.time - _lastMoveTime > 0.5f)
            {
                _lastMoveTime = Time.time;
                if (movePow < 0)
                {
                    print("space");
                    PlayerAnimator.SetTrigger("isFlip");
                    
                    StartCoroutine(flipAnim());
                    PlayerAnimator.SetTrigger("isFlip");


                }
            }
        }

        
        
    }
    private void Swipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            float movePow = touch.deltaPosition.normalized.x;
            if (Mathf.Abs(movePow) > _moveThreshold && Time.time - _lastMoveTime > 0.5f)
            {
                _lastMoveTime = Time.time;
                if (movePow < 0)
                {
                    switch (_lane)
                    {
                        case Lane.First:
                            break;
                        case Lane.Second:
                            //transform.position += new Vector3(_firstLine, 0, 0);
                            moveTo = new Vector3(_firstLine, 0, transform.position.z);
                            _lane = Lane.First;
                            break;
                        case Lane.Third:
                            moveTo = new Vector3(_secondLine, 0, transform.position.z);
                            _lane = Lane.Second;
                            break;
                    }
                }

                if (movePow > 0)
                {
                    switch (_lane)
                    {
                        case Lane.First:
                            moveTo = new Vector3(_secondLine, 0, transform.position.z);
                            _lane = Lane.Second;
                            break;
                        case Lane.Second:
                            moveTo = new Vector3(_thirdLine, 0, transform.position.z);
                            _lane = Lane.Third;
                            break;
                        case Lane.Third:
                            break;
                    }
                }
            }
        }
    }
    public void ScoreUpdate()
    {
        
        PlayerPrefs.SetInt("score", _agentNumber);
        UIManager.manager.ScoreUpdate(_agentNumber);
    }
    private void Prepare()
    {
        if(Input.touchCount > 0)
        {
            GameManager.manager.ToMainGame();
            UIManager.manager.HideIntro();     
            PlayerAnimator.SetTrigger("isRunning");
            _running = true;
            

        }
        
    }
    IEnumerator PunchMetal()
    {   yield return new WaitForSeconds(0.40f);
        fireParticle.Play();
        yield return new WaitForSeconds(0.15f);
        PlayerAnimator.SetTrigger("isRunning");
        
       // _health = 1f;
       // _power = _health/10f;
        //UIManager.manager.PowerBar(_power);
    }
    IEnumerator PunchWood()
    { yield return new WaitForSeconds(0.40f);
        if ( _health >= 10f)
        {
            fireParticle.Play();
            _power = 1f;
            UIManager.manager.PowerBar(_power);
        }
        else
        {
            _health=_health+0.5f;
            IncreaseScaleWood(); 
            _power = _health/10f;
       
            UIManager.manager.PowerBar(_power);
            if (_power >= 1)
            {
                fullPowerFire.SetActive(true);
            }        
        }
        yield return new WaitForSeconds(0.15f);
        PlayerAnimator.SetTrigger("isRunning");
        
       
        
        
        
       
    }

   
    IEnumerator PunchWall()
    { yield return new WaitForSeconds(0.40f);
        if ( _health >= 10f)
        {

            fireParticle.Play();
            _power = 1f;
            UIManager.manager.PowerBar(_power);
        }
        else
        {
            _health=_health+2f;
            IncreaseScaleWall(); 
            _power = _health/10f;
            UIManager.manager.PowerBar(_power);
            if (_power >= 1)
            {
                fullPowerFire.SetActive(true);
            }
        }
        yield return new WaitForSeconds(0.15f);
        PlayerAnimator.SetTrigger("isRunning");
        
       
        
       
    }

    void IncreaseScaleWood()
    {
        _leftUp.localScale += new Vector3(0.05f, 0, 0.05f);
        _rightUp.localScale += new Vector3(0.05f, 0, 0.05f);
        _leftDown.localScale += new Vector3(0.05f, 0, 0.05f);
        _rightDown.localScale += new Vector3(0.05f, 0, 0.05f);
        _leftDownFoot.localScale -= new Vector3(0.025f, 0.025f, 0.025f);
        _rightDownFoot.localScale -= new Vector3(0.025f, 0.025f, 0.025f);
        
    }
    void IncreaseScaleWall()
    {
        _leftUp.localScale += new Vector3(0.1f, 0, 0.1f);
        _rightUp.localScale += new Vector3(0.1f, 0, 0.1f);
        _leftDown.localScale += new Vector3(0.1f, 0, 0.1f);
        _rightDown.localScale += new Vector3(0.1f, 0, 0.1f);
        _leftDownFoot.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
        _rightDownFoot.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
        
    }
   
    IEnumerator Fall()
    {
        
        if (_health >= 3f)
        {
            
            _fall = true;
             starEfect.SetActive(true);
            yield return new WaitForSeconds(2.2f);
            PlayerAnimator.SetTrigger("isRunning");
            _running = true;
            _fall = false;
            starEfect.SetActive(false);
            switch (_lane)
            {
                case Lane.First:
                    moveTo = new Vector3(_thirdLine, 0, 0);
                    _lane = Lane.Third;
                    break;
                case Lane.Second:
                    //transform.position += new Vector3(_firstLine, 0, 0);
                    if (Random.Range(0f, 1f) > 0.5f)
                    {
                        moveTo = new Vector3(_firstLine, 0, 0);
                        _lane = Lane.First;
                    }
                    else
                    {
                        moveTo = new Vector3(_thirdLine, 0, 0);
                        _lane = Lane.Third;
                    }

                    break;
                case Lane.Third:
                    moveTo = new Vector3(_firstLine, 0, 0);
                    _lane = Lane.First;
                    break;
            } 
           
            yield return new WaitForSeconds(0.5f);
            if (_health<5f)
            {
                _health = 0f;
                _power = 0f;
            }
            else
            {
                _health=_health-5f;
                _power = _health/10f;
            }
            
            UIManager.manager.PowerBar(_power);
            _player.tag = "Player";
            
            
        }
        else
        {   atractionArea.SetActive(false);
            starEfect.SetActive(true);
            ///GameOver!!!!!!!!!!!
            _fall = true;
            GameManager.manager.ToFinishGame();
            UIManager.manager.RetryMethod();
           
        }
        
        
        
        
        
    }

    IEnumerator flipAnim()
    {
        yield return new WaitForSeconds(_time);
        //_rigidbody.AddForce(new Vector3(0, _yJump, _zJump)*_jumpForce, ForceMode.Impulse);
        //_rigidbody.velocity += Vector3.up * Physics.gravity.y * (_jumpForce - 1) * Time.deltaTime;
       // _rigidbody.AddForce(Vector3.up*_jumpForce,ForceMode.VelocityChange);
       moveTo = new Vector3(x:0,y:100f,transform.position.z);
    }
    
}
