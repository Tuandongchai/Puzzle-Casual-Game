using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class BoltController : MonoBehaviour
{
    public static BoltController instance;

    [SerializeField] private GameSettings gameSettings;

    private Nut selectedNut;

    private Bolt firstBolt, secondBolt;

    public bool isBusy=false;

    private bool isDragging= false;

    //this stack stores the player's operation during play
    private Stack<Dictionary<List<Bolt>,Nut>> oldStep = new Stack<Dictionary<List<Bolt>, Nut>>();

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Setup(GameSettings settings)
    {
        gameSettings = settings;

    }
    private void Start()
    {
        List<eNutColor> list = new List<eNutColor> { 
            eNutColor.YELLOW, eNutColor.YELLOW, eNutColor.RED, eNutColor.YELLOW
        };
        Bolt bolt = new Bolt(this.transform,new Vector3(-2,1,1), gameSettings, list);

        List<eNutColor> list1 = new List<eNutColor> {
            eNutColor.YELLOW
        };
        Bolt bolt1 = new Bolt(this.transform,Vector3.one, gameSettings, list1);

        List<eNutColor> list2 = new List<eNutColor> {
            eNutColor.YELLOW, eNutColor.RED
        };
        Bolt bolt2 = new Bolt(this.transform, new Vector3(-5, 1, 1), gameSettings, list2);

        List<eNutColor> list3 = new List<eNutColor> {
            eNutColor.RED, eNutColor.YELLOW, eNutColor.RED
        };
        Bolt bolt3 = new Bolt(this.transform, new Vector3(4, 1, 1), gameSettings, list3);

        Bolt bolt4 = new Bolt(this.transform, new Vector3(7, 1, 1), gameSettings, new List<eNutColor>());
    }
    private void Update()
    {
        if (isBusy) return;
        Swap();
        if(Input.GetKeyDown(KeyCode.Space))
            Undo();
    }

    private async void Swap()
    {
        //var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); Game 2D
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (!isDragging)
                {
                    firstBolt = hit.collider.GetComponent<BoltHodler>().boltData;
                    if (firstBolt.GetBoltState() == eBoltState.SIMILAR_FULL) return;
                    if (firstBolt.GetBoltState() == eBoltState.NONE) return;
                    selectedNut = firstBolt.GetTopNut();
                    await NutMoveUp(selectedNut, firstBolt);
                    isDragging = true;
                }
                else
                {
                    secondBolt = hit.collider.GetComponent<BoltHodler>().boltData;
                    Match();
                }
            }
        }
    }

    private void ResetSelectedBolt()
    {
        firstBolt.GetBoltState();
        secondBolt.GetBoltState();
        isDragging = false;
        firstBolt = null;
        secondBolt = null;
        selectedNut = null;
    }
    private void ResetSecondBolt()
    {
        secondBolt = null;
    }
    private async void Match()
    {
        if (await secondBolt.AddTopNut(selectedNut))
        {
            AddStepToOldStep();
            ResetSelectedBolt();
        }
        else
            ResetSecondBolt();
    }

    //undo
    private void AddStepToOldStep()
    {
        Dictionary<List<Bolt>, Nut> dict = new Dictionary<List<Bolt>, Nut>();
        dict[new List<Bolt> { firstBolt, secondBolt }] = selectedNut;
        oldStep.Push(dict);
    }

    public async void Undo()
    {
        if (oldStep.Count <= 0) return;

        Dictionary<List<Bolt>, Nut> dict = new Dictionary<List<Bolt>, Nut>();
        dict = oldStep.Pop();
        List<Bolt> bolts = new List<Bolt>(dict.Keys.First());
        Nut nut = dict.Values.First();
        await NutMoveUp(nut, bolts[1]);
        bolts[1].GetTopNut();
        await bolts[0].NutComeBack(nut);
        bolts[0].GetBoltState();
        bolts[1].GetBoltState();

    }



    //animation
    //move up
    public async Task NutMoveUp(Nut nut, Bolt bolt)
    {
        await AnimateNutMoveUp(nut, bolt);
    }
    private async Task AnimateNutMoveUp(Nut nut, Bolt bolt)
    {
        isBusy = true; 
        nut.AnimateClockwise(bolt.availablePos[0], 0.5f);
        await Task.Delay(500);
        isBusy = false;
    }
    //move to target
    public async void NutMoveToNewBolt(Nut nut,Bolt bolt ,Vector3 newBolt)
    {
        await AnimateNutMoveNewBolt(nut, bolt,newBolt);
    }
    private async Task AnimateNutMoveNewBolt(Nut nut, Bolt bolt,Vector3 newBolt)
    {
        isBusy = true;
        nut.AnimateMove(bolt.availablePos[0], 0.5f);
        await Task.Delay(500);
        nut.Animatecounterclockwise(newBolt, 0.5f);
        await Task.Delay(500);
        isBusy = false;
    }
    // move back

    public async void NutComeBack(Nut nut, Vector3 newBolt)
    {
        await AnimateNutComeBackt(nut, newBolt);
    }
    private async Task AnimateNutComeBackt(Nut nut, Vector3 newBolt)
    {
        isBusy = true;
        nut.Animatecounterclockwise(newBolt, 0.5f);
        await Task.Delay(500);
        isBusy = false;
    }
}
