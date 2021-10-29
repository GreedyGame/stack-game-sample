using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


[RequireComponent(typeof(AudioSource))]
public class TheStackver2 : MonoBehaviour
{
    public AudioClip otherClip,perfectSound;

    public TextMeshProUGUI scoreText;   
    public GameObject rewardPanel,theStackParent;

    public Material stacKMat;
    Color presentCubeColor;

    private const float BOUNDS_SIZE = 3.5f;
    private const float STACK_MOVING_SPEED = 5.0f;
    private const float ERROR_MARGIN = 0.1f;
    private const float STACK_BOUNDS_GAIN = 0.25f;
    private const int COMBO_START_GAIN = 3;

    public GameObject[] theStack;
    private Vector2 stackBounds = new Vector2(BOUNDS_SIZE, BOUNDS_SIZE);
    private Vector2 previousStackBounds;

    private int stackIndex;
    private int scoreCount = 0,bonus=1;
    public static int Score=0;
    private int combo = 0;

    private float tileTransition = 0.0f;
    private float tileSpeed = 2.5f;
    private float secondaryPosition;
    private float xOffset;

    private bool isMovingOnX = true;
    private bool gameOver = false;

    private Vector3 desiredPosition;
    private Vector3 cameraPosition;
    private Vector3 lastTilePosition;
    private AudioSource audio;
    public GameObject WorldCanvas, perfectTxt;
    public TextMeshProUGUI plusOneTxt;
    private int HsvCount=0;

    // Use this for initialization
    private void Start()
    {       
        audio = theStackParent.GetComponent<AudioSource>();
        audio.clip = otherClip;
        cameraPosition = transform.position;
        HsvCount = Random.Range(0, 20);
        theStack = new GameObject[theStackParent.transform.childCount];
        for (int i = 0; i < theStackParent.transform.childCount; i++)
        {
            theStack[i] = theStackParent.transform.GetChild(i).gameObject;
            HsvCount +=10;
            theStack[i].GetComponent<Renderer>().material.color = Color.HSVToRGB((HsvCount) / 360f, 71 / 100f, 55 / 100f);
            if (i == theStackParent.transform.childCount-1)
            {
                presentCubeColor = theStack[i].GetComponent<Renderer>().material.color;
            }
           
        }
        stackIndex = theStackParent.transform.childCount - 1;

        xOffset = transform.position.x;
    }

    private void CreateRubble(Vector3 pos, Vector3 scale)
    {
        audio.clip = otherClip;
        audio.Play();
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.localPosition = pos;        
        go.transform.localScale = scale;
        go.AddComponent<Rigidbody>();
        go.GetComponent<MeshRenderer>().material = stacKMat;
        go.GetComponent<Renderer>().material.color = presentCubeColor;
    }

    
    private void Update()
    {
        if (gameOver)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (PlaceTile())
            {
                SpawnTile();
                scoreCount++;
                Score++;
                Debug.Log(Score + " down" + scoreCount);               
                // scoreText.text = scoreCount.ToString();
            }
            else
            {
                EndGame();
            }
        }

        MoveTile();

        //Move The Stack
        theStackParent.transform.position = Vector3.Lerp(theStackParent.transform.position, desiredPosition, STACK_MOVING_SPEED * Time.deltaTime);
        transform.position= Vector3.Lerp(transform.position, cameraPosition, STACK_MOVING_SPEED * Time.deltaTime);
        Debug.Log(cameraPosition);
    }
  

        private void MoveTile()
    {
        tileTransition += Time.deltaTime * tileSpeed;
        if (isMovingOnX)
        {
            theStack[stackIndex].transform.localPosition = new Vector3(Mathf.Sin(tileTransition) * BOUNDS_SIZE, scoreCount, secondaryPosition);
           
        }
        else
        {
            theStack[stackIndex].transform.localPosition = new Vector3(secondaryPosition, scoreCount, Mathf.Sin(tileTransition) * BOUNDS_SIZE);
            
        }
    }

    private void SpawnTile()
    {
        lastTilePosition = theStack[stackIndex].transform.localPosition;
        stackIndex--;
        if (stackIndex < 0)
        {
            stackIndex = theStackParent.transform.childCount - 1;
        }
        desiredPosition = Vector3.down* scoreCount;
        cameraPosition = new Vector3(lastTilePosition.x + xOffset, transform.position.y, transform.position.z);
        HsvCount +=10;
        if (HsvCount >= 360)
        {
            HsvCount = Random.Range(0, 20);
        }
        theStack[stackIndex].GetComponent<Renderer>().material.color = Color.HSVToRGB((HsvCount)/360f, 71/100f, 55/100f);
        presentCubeColor = theStack[stackIndex].GetComponent<Renderer>().material.color;
        theStack[stackIndex].transform.localPosition = new Vector3(0, scoreCount, 0);
        theStack[stackIndex].transform.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);       
    }

    

    private bool PlaceTile()
    {
        Transform t = theStack[stackIndex].transform;
        previousStackBounds = new Vector2(stackBounds.x, stackBounds.y);
        if (isMovingOnX)

        #region isMovingX

        {
            float deltaX = lastTilePosition.x - t.position.x;
            if (Mathf.Abs(deltaX) > ERROR_MARGIN)
            {
                //CUT THE TILE
                combo = 0;
                bonus = 1;
                stackBounds.x -= Mathf.Abs(deltaX);
                if (stackBounds.x <= 0)
                {
                    return false;
                }                             
                t.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);
                CreateRubble
                (
                    new Vector3((t.position.x > 0)
                    ? t.position.x + (t.localScale.x / 2)
                    : t.position.x - (t.localScale.x / 2)
                    , t.position.y
                    , t.position.z),
                    new Vector3(Mathf.Abs(deltaX), 1, t.localScale.z)
                );
                t.localPosition = new Vector3((lastTilePosition.x + t.localPosition.x) / 2, scoreCount, lastTilePosition.z);
                t.transform.GetChild(0).gameObject.SetActive(true);
                PlusOneAnimation(t.transform,bonus);                
            }
            else
            {
                combo++;
                bonus = bonus * 2;
                if (combo > COMBO_START_GAIN)
                {
                    stackBounds.x += STACK_BOUNDS_GAIN;
                    if (stackBounds.x > BOUNDS_SIZE)
                    {
                        stackBounds.x = BOUNDS_SIZE;
                    }
                    float middle = lastTilePosition.x + t.localPosition.x / 2;
                    t.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

                    t.localPosition = new Vector3(middle - (lastTilePosition.x / 2), scoreCount, lastTilePosition.z);
                }
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, lastTilePosition.z);
                t.transform.GetChild(1).gameObject.SetActive(true);
                PlusOneAnimation(t.transform,bonus);
                audio.clip = perfectSound;
                audio.Play();
            }
        }

        #endregion isMovingX

        else

        #region else

        {
            float deltaZ = lastTilePosition.z - t.position.z;
            if (Mathf.Abs(deltaZ) > ERROR_MARGIN)
            {
                //CUT THE TILE
                combo = 0;
                bonus = 1;
                stackBounds.y -= Mathf.Abs(deltaZ);
                if (stackBounds.y <= 0)
                {
                    return false;
                }

                float middle = lastTilePosition.z + t.localPosition.z / 2;
                t.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);
                CreateRubble
                   (
                       new Vector3(t.position.x
                       , t.position.y
                       , (t.position.z > 0)
                       ? t.position.z + (t.localScale.z / 2)
                       : t.position.z - (t.localScale.z / 2)),
                       new Vector3(t.localScale.x, Mathf.Abs(deltaZ), 1)
                   );                
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, middle - (lastTilePosition.z / 2));
                t.transform.GetChild(0).gameObject.SetActive(true);
                PlusOneAnimation(t.transform, bonus);

            }
            else
            {
                combo++;
                bonus = bonus * 2;
                if (combo > COMBO_START_GAIN)
                {
                    stackBounds.x += STACK_BOUNDS_GAIN;
                    if (stackBounds.y > BOUNDS_SIZE)
                    {
                        stackBounds.y = BOUNDS_SIZE;
                    }
                    float middle = lastTilePosition.z + t.localPosition.z / 2;
                    t.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);
                    t.localPosition = new Vector3(lastTilePosition.x, scoreCount, middle - (lastTilePosition.z / 2));
                }
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, lastTilePosition.z);
                t.transform.GetChild(1).gameObject.SetActive(true);
                PlusOneAnimation(t.transform,bonus);
                audio.clip = perfectSound;
                audio.Play();
            }
        }

        #endregion else

        secondaryPosition = (isMovingOnX)
             ? t.localPosition.x
            : t.localPosition.z;

        isMovingOnX = !isMovingOnX;
        return true;
    }
    

    private void EndGame()
    {      
       // PlayerPrefs.SetInt("score", scoreCount);
       
        gameOver = true;
        theStackParent.SetActive(false);
        rewardPanel.SetActive(true);

        //theStack[stackIndex].AddComponent<Rigidbody>();
    }
    public void Watch()
    {
        Debug.Log("Watch");
        Admanager.Instance.ShowReward();        
    }
    public void Rewards()
    {
        gameOver = false;
        rewardPanel.SetActive(false);
        theStackParent.SetActive(true);
        float x = Mathf.Clamp(theStack[stackIndex].transform.localScale.x * 1.5f, 0.5f, BOUNDS_SIZE);
        float z = Mathf.Clamp(theStack[stackIndex].transform.localScale.z * 1.5f, 0.5f, BOUNDS_SIZE);
        theStack[stackIndex].transform.localScale = new Vector3(x, theStack[stackIndex].transform.localScale.y, z);
        if (stackIndex >= theStack.Length-1)
        {
            theStack[0].transform.localScale = new Vector3(x, theStack[0].transform.localScale.y, z);
        }
        else
        {
            theStack[stackIndex + 1].transform.localScale = new Vector3(x, theStack[stackIndex + 1].transform.localScale.y, z);
        }
       
        stackBounds = new Vector2(x,z);        
        
    }
    public void Cancel()
    {
        SceneManager.LoadScene("GameOver");
        Debug.Log("Cancel");
    }

    public void OnButtonClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }   
    void PlusOneAnimation(Transform t,int bonus)
    {
        if (bonus > 1)
        {
            plusOneTxt.text = "+" + bonus;
            plusOneTxt.color = Color.yellow;
            plusOneTxt.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            Score += (bonus-1);
            perfectTxt.SetActive(true);
            perfectTxt.transform.localScale = Vector3.one;
            LeanTween.scale(perfectTxt, perfectTxt.transform.localScale * 1.8f, 0.7f).setEaseOutBounce().setOnComplete(() => { perfectTxt.SetActive(false); });
        }
        else
        {
            plusOneTxt.text = "+" + bonus;
            plusOneTxt.color = Color.white;
            plusOneTxt.transform.localScale = Vector3.one;
        }
        WorldCanvas.SetActive(true);
        WorldCanvas.transform.position = t.transform.position;
        LeanTween.move(WorldCanvas, new Vector3(0, 4.2f, 0), 1f).setOnComplete(() => { scoreText.text = Score.ToString(); });
        LeanTween.alpha(WorldCanvas, 0, 1f).setOnComplete(() => { WorldCanvas.SetActive(false); });
    }
}