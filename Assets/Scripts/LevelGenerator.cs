using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LevelGenerator : MonoBehaviour
{
    public int GridSize = 12;
    public GameObject Floor;
    public GameObject Wall;
    public Color mycolorR;
    public Color mycolorD;
    public Color mycolorF;
    private GridPoint Sp;
    private GridPoint Ep;
    private List<GridPoint> RoutePoints;
    private GameObject[,] Grid;
    public GameObject Player;
    /*public static LevelGenerator instance;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }*/
    // Start is called before the first frame update
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        GenerateMap();
        GenerateTrap();
        GenerateRoad();
        GenerateOther();
        GameObject player;
        player = Instantiate(Player,Vector3.zero, Quaternion.identity);
        player.transform.position = new Vector3((Floor.transform.localScale.x * Sp.x), 0.3f, -3f);
        player.transform.GetChild(player.transform.childCount-1).gameObject.GetComponent<Camera>().enabled = true;
        GameObject endGameTrigger = new GameObject("EndGameTrigger");
        var endGameBoxCollider = endGameTrigger.AddComponent<BoxCollider>();
        endGameTrigger.AddComponent<EndGameTrigger>();
        endGameBoxCollider.isTrigger = true;
        endGameTrigger.transform.position = new Vector3(GridSize * 1.5f, -2f, GridSize * 1.5f);
        endGameBoxCollider.size = new Vector3((GridSize+1) * 3f, 0.5f, (GridSize+1) * 3f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public class GridPoint
    {
        public GridPoint(int yy, int xx, char vall, double gg, GridPoint End)
        {
            y = yy;
            x = xx;
            val = vall;
            g = gg;
            h = Mathf.Sqrt(Mathf.Pow(x - End.x, 2) + Mathf.Pow(y - End.y, 2));
            f = g + h;
        }
        public GridPoint (int yy, int xx)
        {
            y = yy;
            x = xx;
        }
        public int x { get; set; }
        public int y { get; set; }
        public char val { get; set; }
        public double g { get; set; }
        public double h { get; set; }
        public double f { get; set; }
        public GridPoint parent { get; set; }
    }
    private void GenerateMap()
    {
        float size = Floor.transform.localScale.x;
        Grid = new GameObject[GridSize, GridSize];
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                Grid[i, j] = Instantiate(Floor, new Vector3(j * size, 0, i * size), Quaternion.identity);
                Grid[i, j].name = "Floor_" + i + "_" + j;
                /*if (i == GridSize - 1 && j == GridSize - 1) //stara kamera
                {
                    Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
                    cam.transform.position += new Vector3((GridSize - 1) / 2 * size, (GridSize + 1) * size, (GridSize + 1) / 2 * size);
                    cam.transform.LookAt(Vector3.zero + new Vector3((GridSize - 1) / 2 * size, 0, (GridSize - 1) / 2 * size));
                }*/
            }
        }
        int efloori = UnityEngine.Random.Range(0, GridSize - 1);
        int sfloori = UnityEngine.Random.Range(0, GridSize - 1);
        GameObject efloor = Instantiate(Floor, new Vector3(efloori * size, 0, GridSize * size), Quaternion.identity);
        efloor.name = "End_Floor";
        efloor.AddComponent<EndLevelTrigger>();
        for(int i=1;i<5;i++)
        {
            GameObject eroute = Instantiate(Floor, new Vector3(efloori * size, 0, (GridSize * size) + (i* size)), Quaternion.identity);
            eroute.tag = "DoNotMove";
        }
        GameObject sfloor = Instantiate(Floor, new Vector3(sfloori * size, 0, -size), Quaternion.identity);
        sfloor.name = "Start_Floor";
        for (int i = 1; i < 5; i++)
        {
            GameObject sroute = Instantiate(Floor, new Vector3(sfloori * size, 0, -size - (i * size)), Quaternion.identity);
            sroute.tag = "DoNotMove";
        }
        for (int i = -1; i < GridSize + 1; i++)
        {
            if(sfloori != i ) Instantiate(Wall, new Vector3(i * size, 2.5f, -3f), Quaternion.identity);
            else
            {
                for(int j = 1; j < 5; j++)
                {
                    Instantiate(Wall, new Vector3((i-1) * size, 2.5f, -3f-(j*size)), Quaternion.identity);
                    Instantiate(Wall, new Vector3((i + 1) * size, 2.5f, -3f - (j * size)), Quaternion.identity);
                }
            }
        }
        for (int i = -1; i < GridSize + 1; i++)
        {
            if (efloori != i) Instantiate(Wall, new Vector3(i * size, 2.5f, GridSize * size), Quaternion.identity);
            else
            {
                for (int j = 1; j < 5; j++)
                {
                    Instantiate(Wall, new Vector3((i - 1) * size, 2.5f, GridSize * size + (j * size)), Quaternion.identity);
                    Instantiate(Wall, new Vector3((i + 1) * size, 2.5f, GridSize * size + (j * size)), Quaternion.identity);
                }
            }
        }
        for (int i = 0; i < GridSize; i++)
        {
            Instantiate(Wall, new Vector3(GridSize * size, 2.5f, i* size), Quaternion.identity);
        }
        for (int i = 0; i < GridSize; i++)
        {
            Instantiate(Wall, new Vector3(-3, 2.5f, i * size), Quaternion.identity);
        }
        Sp = new GridPoint(0, sfloori);
        Ep = new GridPoint(GridSize - 1, efloori);
        RoutePoints = new List<GridPoint>();
        List<GridPoint> Points = new List<GridPoint>();
        Points.Add(Sp);
        GridPoint p1 = null;
        for (int i = 0; i < 5; i++)
        {
            do
            {
                p1 = new GridPoint(UnityEngine.Random.Range(1, GridSize - 2), UnityEngine.Random.Range(0, GridSize - 1));
            }
            while (Points.Contains(p1) && p1 != Ep);
            Points.Add(p1);
        }
        Points.Add(Ep);
        Points = Points.OrderBy(o => o.y).ToList();
        for (int i = 1; i < Points.Count; i++) Route(Points[i - 1], Points[i]);
    }
    private void GenerateRoad()
    {
        foreach (GridPoint t in RoutePoints)
        {
            if(Grid[t.y, t.x].GetComponent<Renderer>().material.color != mycolorR)
                Grid[t.y, t.x].GetComponent<Renderer>().material.color = mycolorF;
        }
    }
    private void GenerateOther()
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                if (!Contains(RoutePoints, i, j))
                {
                    if (i > GridSize / 2 && Border(i, j))
                    {
                        Grid[i, j].GetComponent<Renderer>().material.color = mycolorD;
                        Grid[i, j].AddComponent<FloorTriger>();
                    }
                    else
                    {
                        switch (UnityEngine.Random.Range(0, 2))
                        {
                            case 0:
                                Grid[i, j].GetComponent<Renderer>().material.color = mycolorD;
                                Grid[i, j].AddComponent<FloorTriger>();
                                break;
                            case 1:
                                Grid[i, j].GetComponent<Renderer>().material.color = mycolorF;
                                break;
                        }
                    }
                }
            }
        }
    }
    private void GenerateTrap()
    {
        GridPoint Switch;
        GridPoint Trap;
        do
        {
            Switch = new GridPoint(UnityEngine.Random.Range(0, GridSize / 2), UnityEngine.Random.Range(0, GridSize));
        }
        while (Contains(RoutePoints, Switch.y, Switch.x));
        RoutePoints.Sort((x, y) => x.y.CompareTo(y.y));
        int Countdown = 0;
        do
        {
            Trap = RoutePoints[RoutePoints.Count - 1 - Countdown];
            Countdown++;
            //Trap = new GridPoint(UnityEngine.Random.Range(GridSize / 2 + 1, GridSize), UnityEngine.Random.Range(0, GridSize));
        }
        while ((BorderCount(Trap.y,Trap.x)!=3));
        foreach(GridPoint t in RoutePoints)
        {
            if(t.y == Switch.y)
            {
                Route(Switch, t);
                break;
            }
        }
        Grid[Trap.y, Trap.x].transform.position = Grid[Trap.y, Trap.x].transform.position + new Vector3(0, -5, 0);
        var bc = Grid[Switch.y, Switch.x];
        bc.GetComponent<Renderer>().material.color = mycolorR;
        var trapComponent = bc.AddComponent<Trigger>();
        trapComponent.Trap = Grid[Trap.y, Trap.x];
        trapComponent.clip = gameObject.GetComponent<Sounds>().floorMoveSound;
        trapComponent.dust = gameObject.GetComponent<Effects>().dust;

        //var triggergo = new GameObject();
        //triggergo.AddComponent<BoxCollider>().isTrigger = true;
        //var tc = triggergo.AddComponent<Trigger>();
        //tc.Trap = Grid[Trap.y, Trap.x];
        //triggergo.transform.parent = bc.transform;
        //triggergo.transform.localPosition = new Vector3(0, 1, 0);

    }
    public bool Contains(List<GridPoint> lista, int y, int x)
    {
        foreach (GridPoint p in lista)
            if (p.x == x && p.y == y)
                return true;
        return false;
    }
    private bool Border(int y, int x)
    {
        foreach(GridPoint t in RoutePoints)
        {
            if ((y >= t.y - 1) && (y <= t.y + 1) && (x >= t.x - 1) && (x <= t.x + 1)) return true;
        }
        return false;
    }
    private int BorderCount(int y, int x)
    {
        int i = 0;
        for (int j = -1; j < 2; j++)
            for (int k = -1; k < 2; k++)
                if (!(k == 0 && j == 0) && (Contains(RoutePoints, y + j, x + k))) i++;
        /*if (Contains(RoutePoints, y, x + 1)) i++;
        if (Contains(RoutePoints, y, x - 1)) i++;
        if (Contains(RoutePoints, y + 1, x)) i++;
        if (Contains(RoutePoints, y - 1, x)) i++;*/
        return i;
    }
    public bool Route(GridPoint Start, GridPoint End)
    {
        char[,] tab = new char[GridSize, GridSize];
        for (int i = 0; i < GridSize; i++)
            for (int j = 0; j < GridSize; j++)
                tab[i, j] = '0';
        List<GridPoint> O = new List<GridPoint>();
        List<GridPoint> C = new List<GridPoint>();
        O.Add(Start);
        while (true)
        {
            GridPoint cur = O[0];
            for (int i = 0; i < O.Count; i++)
            {
                if (O[i].f < cur.f)
                    cur = O[i];
            }
            C.Add(cur);
            O.Remove(cur);
            if (cur.x == End.x && cur.y == End.y)
                break;
            if (cur.y + 1 < GridSize && tab[cur.y + 1, cur.x] != 'x' && !Contains(C, cur.y + 1, cur.x) && !Contains(O, cur.y + 1, cur.x))
            {
                GridPoint temp = new GridPoint(cur.y + 1, cur.x, tab[cur.y + 1, cur.x], cur.g + 1.0, End);
                temp.parent = cur;
                O.Add(temp);
            }
            if (cur.y - 1 >= 0 && tab[cur.y - 1, cur.x] != 'x' && !Contains(C, cur.y - 1, cur.x) && !Contains(O, cur.y - 1, cur.x))
            {
                GridPoint temp = new GridPoint(cur.y - 1, cur.x, tab[cur.y - 1, cur.x], cur.g + 1.0, End);
                temp.parent = cur;
                O.Add(temp);
            }
            if (cur.x + 1 < GridSize && tab[cur.y, cur.x+1] != 'x' && !Contains(C, cur.y , cur.x+1) && !Contains(O, cur.y , cur.x+1))
            {
                GridPoint temp = new GridPoint(cur.y , cur.x+1, tab[cur.y , cur.x+1], cur.g + 1.0, End);
                temp.parent = cur;
                O.Add(temp);
            }
            if (cur.x - 1 >= 0 && tab[cur.y, cur.x - 1] != 'x' && !Contains(C, cur.y, cur.x - 1) && !Contains(O, cur.y, cur.x - 1))
            {
                GridPoint temp = new GridPoint(cur.y, cur.x - 1, tab[cur.y, cur.x - 1], cur.g + 1.0, End);
                temp.parent = cur;
                O.Add(temp);
            }
            if (O.Count == 0)
                return false;
        }
        GridPoint tempe = C[C.Count - 1];
        List<GridPoint> templist = new List<GridPoint>();
        while (tempe != null)
        {
            templist.Add(new GridPoint(tempe.y, tempe.x));
            tempe = tempe.parent;
        }
        RoutePoints = RoutePoints.Union(templist).ToList();
        return true;
    }
}
