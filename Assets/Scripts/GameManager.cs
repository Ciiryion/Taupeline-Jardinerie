using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxRoomNbr = 15;
    private List<Room> roomTab;
    public static GameManager instance;

    [Header("Paramètre d'une room")]
    [SerializeField] private float roomWidth = 10f; // Largeur de la salle (Axe X)
    [SerializeField] private float roomHeight = 10f; // Profondeur de la salle (Axe Z ou Y)
    [SerializeField] private Transform dungeonParent; // Un objet vide pour ranger le donjon

    [Header("Prefabs des Salles (0 à 15)")]
    // Crée un tableau de 16 cases dans l'inspecteur Unity.
    // L'index du tableau correspondra à l'ID calculé par le bitmasking.
    [SerializeField] private GameObject[] roomPrefabs = new GameObject[16];

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomTab = new List<Room>();
        //Création de la room initiale
        List<Room.Direction> sorties = new List<Room.Direction>();
        sorties.Add(Room.Direction.UP);   sorties.Add(Room.Direction.DOWN);
        sorties.Add(Room.Direction.LEFT); sorties.Add(Room.Direction.RIGHT);
        List<bool> isClose = new List<bool>();
        for (int i = 0; i < 4; i++)
            isClose.Add(false);
        roomTab.Add(new Room(0,0, sorties, isClose));

        int roomIdx = 1;
        
        //Boucle en vérifiant le nombre de salle max
        while (roomIdx < maxRoomNbr && (roomIdx - 1) < roomTab.Count)
        {
            //Debug.Log("roomTab[roomIdx - 1].sorties.Count = " + roomTab[roomIdx - 1].sorties.Count);
            //Debug.Log("roomIdx - 1 = " + (roomIdx - 1));
            for (int i = 0; i < roomTab[roomIdx - 1].sorties.Count; i++)
            {
                //Debug.Log("i = " + i);
                if (roomTab[roomIdx - 1].isClose[i] == false)
                {
                    switch (roomTab[roomIdx - 1].sorties[i])
                    {
                        case Room.Direction.UP:
                            //Creation de la nouvelle room
                            Room newRoomUp = createRandomRoom(Room.Direction.DOWN, roomTab[roomIdx - 1]);
                            //Vérification des positions de la room
                            checkRoomPositions(newRoomUp);
                            break;
                        case Room.Direction.DOWN:
                            Room newRoomDown = createRandomRoom(Room.Direction.UP, roomTab[roomIdx - 1]);
                            checkRoomPositions(newRoomDown);
                            break;
                        case Room.Direction.LEFT:
                            Room newRoomLeft = createRandomRoom(Room.Direction.RIGHT, roomTab[roomIdx - 1]);
                            checkRoomPositions(newRoomLeft);
                            break;
                        case Room.Direction.RIGHT:
                            Room newRoomRight = createRandomRoom(Room.Direction.LEFT, roomTab[roomIdx - 1]);
                            checkRoomPositions(newRoomRight);
                            break;
                        default:
                            Debug.Log("Erreur 0001 : direction non existante");
                            break;
                    }
                    roomTab[roomIdx - 1].isClose[i] = true;
                }
            }
            roomIdx++;
        }

        //Quand le nombre de salle max est atteint, fermer les salles encore ouvertes
        int initialRoomCount = roomTab.Count;

        for (int i = 0; i < initialRoomCount; i++)
        {
            for(int j = 0; j < roomTab[i].isClose.Count; j++)
            {
                if(roomTab[i].isClose[j] == false)
                {
                    switch(roomTab[i].sorties[j])
                    {
                        case Room.Direction.UP:
                            List<Room.Direction> endSortieUp = new List<Room.Direction>();
                            endSortieUp.Add(Room.Direction.DOWN);
                            List<bool> endIsCloseUp = new List<bool>();
                            endIsCloseUp.Add(true);
                            Room lastRoomUp = new Room(roomTab[i].x, roomTab[i].y + 1, endSortieUp, endIsCloseUp);
                            checkRoomPositions(lastRoomUp);
                            break;
                        case Room.Direction.DOWN:
                            List<Room.Direction> endSortieDown = new List<Room.Direction>();
                            endSortieDown.Add(Room.Direction.UP);
                            List<bool> endIsCloseDown = new List<bool>();
                            endIsCloseDown.Add(true);
                            Room lastRoomDown = new Room(roomTab[i].x, roomTab[i].y - 1, endSortieDown, endIsCloseDown);
                            checkRoomPositions(lastRoomDown);
                            break;
                        case Room.Direction.LEFT:
                            List<Room.Direction> endSortieLeft = new List<Room.Direction>();
                            endSortieLeft.Add(Room.Direction.RIGHT);
                            List<bool> endIsCloseLeft = new List<bool>();
                            endIsCloseLeft.Add(true);
                            Room lastRoomLeft = new Room(roomTab[i].x - 1, roomTab[i].y, endSortieLeft, endIsCloseLeft);
                            checkRoomPositions(lastRoomLeft);
                            break;
                        case Room.Direction.RIGHT:
                            List<Room.Direction> endSortieRight = new List<Room.Direction>();
                            endSortieRight.Add(Room.Direction.LEFT);
                            List<bool> endIsCloseRight = new List<bool>();
                            endIsCloseRight.Add(true);
                            Room lastRoomRight = new Room(roomTab[i].x + 1, roomTab[i].y, endSortieRight, endIsCloseRight);
                            checkRoomPositions(lastRoomRight);
                            break;
                        default:
                            Debug.Log("Erreur 0005 : Direction inexistante"); break;
                    }
                    roomTab[i].isClose[j] = false;
                }
            }
        }

        // Instancier les salles
        GenerateVisualDungeon();

    }

    private Room createRandomRoom(Room.Direction pDirection, Room currentRoom)
    {
        List<Room.Direction> NextSorties = new List<Room.Direction>();
        List<bool> isClose = new List<bool>();

        float x = currentRoom.x, y = currentRoom.y;
        //Debug.Log("CurrentRoom pos : " + x + " , " + y);

        NextSorties.Add(pDirection);
        isClose.Add(true);

        int probSorties = Random.Range(0, 6);
        if(probSorties == 0)
        {
            //Debug.Log("Direction NONE");
            switch(pDirection)
            {
                case Room.Direction.UP:
                    y -= 1;
                    break;
                case Room.Direction.DOWN:
                    y += 1;
                    break;
                case Room.Direction.LEFT:
                    x += 1;
                    break;
                case Room.Direction.RIGHT:
                    x -= 1;
                    break;
                default:
                    Debug.Log("Erreur 0002 : Direction Inexistante"); break;

            }
        }
        else
        {
            int nextDirection = Random.Range(1, 3);

            switch (pDirection)
            {
                case Room.Direction.UP:
                    switch(nextDirection)
                    {
                        case 1: 
                            NextSorties.Add(Room.Direction.DOWN);
                            isClose.Add(false);
                            break;
                        case 2:
                            NextSorties.Add(Room.Direction.RIGHT);
                            isClose.Add(false);
                            break;
                        case 3:
                            NextSorties.Add(Room.Direction.LEFT);
                            isClose.Add(false);
                            break;
                        default: Debug.Log("Erreur 0003 : Prochaine direction impossible (UP)"); break;
                    }
                    y -= 1;
                    break;
                case Room.Direction.DOWN:
                    switch (nextDirection)
                    {
                        case 1:
                            NextSorties.Add(Room.Direction.UP);
                            isClose.Add(false);
                            break;
                        case 2:
                            NextSorties.Add(Room.Direction.RIGHT);
                            isClose.Add(false);
                            break;
                        case 3:
                            NextSorties.Add(Room.Direction.LEFT);
                            isClose.Add(false);
                            break;
                        default: Debug.Log("Erreur 0003 : Prochaine direction impossible (DOWN)"); break;
                    }
                    y += 1;
                    break;
                case Room.Direction.RIGHT:
                    switch (nextDirection)
                    {
                        case 1:
                            NextSorties.Add(Room.Direction.UP);
                            isClose.Add(false);
                            break;
                        case 2:
                            NextSorties.Add(Room.Direction.DOWN);
                            isClose.Add(false);
                            break;
                        case 3:
                            NextSorties.Add(Room.Direction.LEFT);
                            isClose.Add(false);
                            break;
                        default: Debug.Log("Erreur 0003 : Prochaine direction impossible (RIGHT)"); break;
                    }
                    x -= 1;
                    break;
                case Room.Direction.LEFT:
                    switch (nextDirection)
                    {
                        case 1:
                            NextSorties.Add(Room.Direction.UP);
                            isClose.Add(false);
                            break;
                        case 2:
                            NextSorties.Add(Room.Direction.DOWN);
                            isClose.Add(false);
                            break;
                        case 3:
                            NextSorties.Add(Room.Direction.RIGHT);
                            isClose.Add(false);
                            break;
                        default: Debug.Log("Erreur 0003 : Prochaine direction impossible (LEFT)"); break;
                    }
                    x += 1;
                    break;
                default: Debug.Log("Erreur 0004 : Direction inexistante");
                    break;
            }
        }
        //Debug.Log("newRoomPos = " + x + " , " + y);
        return new Room(x, y, NextSorties, isClose);
    }

    private void checkRoomPositions(Room newRoom)
    {
        foreach(Room room in roomTab)
        {
            if(newRoom.x == room.x && newRoom.y == room.y)
            {
                //Debug.Log("2 room meme endroit");

                bool isExist = false;
                // Modifier room pour y ajouter les sorties de newRoom
                for (int i = 0; i < newRoom.sorties.Count; i++)
                {
                    isExist = false;
                    for(int j = 0; j < room.sorties.Count; j++)
                    {
                        if (room.sorties[j] == newRoom.sorties[i])
                        {
                            isExist = true;
                        }
                    }
                    if(!isExist)
                    {
                        //newRoom.isClose[i] = true;
                        room.sorties.Add(newRoom.sorties[i]);
                        room.isClose.Add(newRoom.isClose[i]);
                    }
                }

                return;
            }
        }

        // Ajouter newRoom dans roomTab si il n'y a pas de room au meme endroit
        roomTab.Add(newRoom);
        return;
    }

    private void GenerateVisualDungeon()
    {
        foreach (Room room in roomTab)
        {
            // Calcul de l'ID unique de la salle
            int roomID = CalculateRoomID(room);

            if (roomPrefabs[roomID] == null)
            {
                Debug.LogWarning("Erreur 0006 le prefab pour l'ID " + roomID + " est manquant");
                continue;
            }

            Vector3 spawnPosition = new Vector3(room.x * roomWidth, room.y * roomHeight, 0);
            Instantiate(roomPrefabs[roomID], spawnPosition, Quaternion.identity, dungeonParent);
        }
    }

    // Fonction pour obtenir l'ID de 0 à 15
    private int CalculateRoomID(Room room)
    {
        int id = 0;

        for (int i = 0; i < room.sorties.Count; i++)
        {
            switch (room.sorties[i])
            {
                case Room.Direction.UP: id += 1; break;
                case Room.Direction.RIGHT: id += 2; break;
                case Room.Direction.DOWN: id += 4; break;
                case Room.Direction.LEFT: id += 8; break;
            }
        }

        return id;
    }

    /*
    public void addRoomInTab(GameObject room)
    {
        bool roomExist = false;
        for (int i = 0; i < roomTab.Count; i++)
        {
            if (roomTab[i].name == room.name)
            {
                roomExist = true;
            }
        }
        if (!roomExist)
        {
            roomTab.Add(room);
        }
    }
    */
}
