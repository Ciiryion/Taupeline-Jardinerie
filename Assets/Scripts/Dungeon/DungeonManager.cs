using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private GridManager gridManager;

    [SerializeField] private int maxRoomNbr = 15;
    [SerializeField] private int probTaille = 5;
    private List<Room> roomTab;
    private List<GameObject> roomList;

    [Header("Paramètre d'une room")]
    [SerializeField] private float roomWidth = 10f;
    [SerializeField] private float roomHeight = 10f;
    [SerializeField] private Transform dungeonParent;

    [Header("Prefabs des Salles (0 à 15)")]
    // L'index du tableau correspond à l'ID calculé par le bitmasking
    [SerializeField] private GameObject[] roomPrefabs = new GameObject[16];

    [Space]
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private GameObject doorPrefab;

    void Start()
    {
        roomTab = new List<Room>();
        roomList = new List<GameObject>();
        //Création de la room initiale
        List<Room.Direction> sorties = new List<Room.Direction>();
        sorties.Add(Room.Direction.UP);   sorties.Add(Room.Direction.DOWN);
        sorties.Add(Room.Direction.LEFT); sorties.Add(Room.Direction.RIGHT);
        List<bool> isClose = new List<bool>();
        for (int i = 0; i < 4; i++)
            isClose.Add(false);
        roomTab.Add(new Room(0, 0, sorties, isClose, 0));

        int roomIdx = 1;
        
        //Boucle en vérifiant le nombre de salle max
        while (roomIdx < maxRoomNbr && (roomIdx - 1) < roomTab.Count)
        {
            for (int i = 0; i < roomTab[roomIdx - 1].sorties.Count; i++)
            {
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
                    int ennemisNbr = Random.Range(1,7);
                    switch(roomTab[i].sorties[j])
                    {
                        case Room.Direction.UP:
                            List<Room.Direction> endSortieUp = new List<Room.Direction>();
                            endSortieUp.Add(Room.Direction.DOWN);
                            List<bool> endIsCloseUp = new List<bool>();
                            endIsCloseUp.Add(true);
                            Room lastRoomUp = new Room(roomTab[i].x, roomTab[i].y + 1, endSortieUp, endIsCloseUp, ennemisNbr);
                            checkRoomPositions(lastRoomUp);
                            break;
                        case Room.Direction.DOWN:
                            List<Room.Direction> endSortieDown = new List<Room.Direction>();
                            endSortieDown.Add(Room.Direction.UP);
                            List<bool> endIsCloseDown = new List<bool>();
                            endIsCloseDown.Add(true);
                            Room lastRoomDown = new Room(roomTab[i].x, roomTab[i].y - 1, endSortieDown, endIsCloseDown, ennemisNbr);
                            checkRoomPositions(lastRoomDown);
                            break;
                        case Room.Direction.LEFT:
                            List<Room.Direction> endSortieLeft = new List<Room.Direction>();
                            endSortieLeft.Add(Room.Direction.RIGHT);
                            List<bool> endIsCloseLeft = new List<bool>();
                            endIsCloseLeft.Add(true);
                            Room lastRoomLeft = new Room(roomTab[i].x - 1, roomTab[i].y, endSortieLeft, endIsCloseLeft, ennemisNbr);
                            checkRoomPositions(lastRoomLeft);
                            break;
                        case Room.Direction.RIGHT:
                            List<Room.Direction> endSortieRight = new List<Room.Direction>();
                            endSortieRight.Add(Room.Direction.LEFT);
                            List<bool> endIsCloseRight = new List<bool>();
                            endIsCloseRight.Add(true);
                            Room lastRoomRight = new Room(roomTab[i].x + 1, roomTab[i].y, endSortieRight, endIsCloseRight, ennemisNbr);
                            checkRoomPositions(lastRoomRight);
                            break;
                        default:
                            Debug.Log("Erreur 0005 : Direction inexistante"); break;
                    }
                    roomTab[i].isClose[j] = false;
                }
            }
        }

        AssignSpecialRooms();

        // Instancier les salles
        GenerateVisualDungeon();

        // Instancier le joueur apres la generation des salles
        GameObject player = Instantiate(playerPrefab, new Vector2(roomTab[0].x, roomTab[0].y), Quaternion.identity);
        
        RoomManager.instance.SetCameraTarget();
        RoomManager.instance.EnterRoom(roomList[0].gameObject.GetComponent<BoxCollider2D>());

        
        if(gridManager != null)
        {
            int minX, minY, maxX, maxY;
            CalculateDungeonBounds(out minX, out minY, out maxX, out maxY);
            gridManager.CreatePathfindingGrid(minX, minY, maxX, maxY);
        }
        
    }

    private Room createRandomRoom(Room.Direction pDirection, Room currentRoom)
    {
        List<Room.Direction> NextSorties = new List<Room.Direction>();
        List<bool> isClose = new List<bool>();

        float x = currentRoom.x, y = currentRoom.y;

        NextSorties.Add(pDirection);
        isClose.Add(true);

        int probSorties = Random.Range(0, probTaille);
        if(probSorties == 0)
        {
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
            int nextDirection = Random.Range(1, 4);
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
        int ennemisNbr = Random.Range(1, 7);

        //Debug.Log("newRoomPos = " + x + " , " + y);
        return new Room(x, y, NextSorties, isClose, ennemisNbr);
    }

    private void checkRoomPositions(Room newRoom)
    {
        foreach(Room room in roomTab)
        {
            if(newRoom.x == room.x && newRoom.y == room.y)
            {
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
            GameObject obj = Instantiate(roomPrefabs[roomID], spawnPosition, Quaternion.identity, dungeonParent);
            obj.GetComponent<RoomTrigger>().SpawnMobs(room.ennemisNbr);
            roomList.Add(obj);

            // Instantiation de la clé
            if (room.type == Room.Type.KeyRoom)
                Instantiate(keyPrefab, spawnPosition, Quaternion.identity, obj.transform);

            // Instantiation de la porte du magasin
            if (room.type == Room.Type.Shop)
            {
                foreach (Room.Direction dir in room.sorties)
                {
                    Vector3 doorPosition = spawnPosition;
                    Quaternion doorRotation = Quaternion.identity;

                    // On décale la position vers le bord de la salle en fonction de la direction
                    switch (dir)
                    {
                        case Room.Direction.UP:
                            doorPosition += new Vector3(0, roomHeight / 2f, 0);
                            break;
                        case Room.Direction.DOWN:
                            doorPosition += new Vector3(0, -roomHeight / 2f, 0);
                            break;
                        case Room.Direction.LEFT:
                            doorPosition += new Vector3(-roomWidth / 2f, 0, 0);
                            doorRotation = Quaternion.Euler(0, 0, 90f);
                            break;
                        case Room.Direction.RIGHT:
                            doorPosition += new Vector3(roomWidth / 2f, 0, 0);
                            doorRotation = Quaternion.Euler(0, 0, 90f);
                            break;
                    }

                    Instantiate(doorPrefab, doorPosition, doorRotation, obj.transform);
                }
            }
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

    private void CalculateDungeonBounds(out int minWorldX, out int minWorldY, out int maxWorldX, out int maxWorldY)
    {
        if (roomTab.Count == 0)
        {
            minWorldX = minWorldY = maxWorldX = maxWorldY = 0;
            return;
        }

        // Initialisation
        float minRoomX = roomTab[0].x;
        float maxRoomX = roomTab[0].x;
        float minRoomY = roomTab[0].y;
        float maxRoomY = roomTab[0].y;

        // Recherche des limites
        foreach (Room room in roomTab)
        {
            if (room.x < minRoomX) minRoomX = room.x;
            if (room.x > maxRoomX) maxRoomX = room.x;
            if (room.y < minRoomY) minRoomY = room.y;
            if (room.y > maxRoomY) maxRoomY = room.y;
        }

        // Conversion en coordonnées World Space et ajout d'une marge
        float marginX = (roomWidth / 2f) + 2f;
        float marginY = (roomHeight / 2f) + 2f;

        // On arrondit pour avoir des entiers stricts
        minWorldX = Mathf.FloorToInt((minRoomX * roomWidth) - marginX);
        maxWorldX = Mathf.CeilToInt((maxRoomX * roomWidth) + marginX);
        minWorldY = Mathf.FloorToInt((minRoomY * roomHeight) - marginY);
        maxWorldY = Mathf.CeilToInt((maxRoomY * roomHeight) + marginY);

        //Debug.Log($"Limites du donjon calculées : X[{minWorldX} à {maxWorldX}] et Y[{minWorldY} à {maxWorldY}]");
    }

    private void AssignSpecialRooms()
    {
        List<Room> candidateRoom = roomTab.GetRange(1, roomTab.Count - 1);

        // Mélange de la liste des candidats
        for(int i = 0; i < candidateRoom.Count; i++)
        {
            Room temp = candidateRoom[i];
            int rndIdx = Random.Range(i, candidateRoom.Count);
            candidateRoom[i] = candidateRoom[rndIdx];
            candidateRoom[rndIdx] = temp;
        }

        // Tri sur le nombre de sorties pour avoir avant tout les culs de sacs
        candidateRoom.Sort((roomA, roomB) => roomA.sorties.Count.CompareTo(roomB.sorties.Count));


        candidateRoom[0].type = Room.Type.Shop;
        candidateRoom[0].ennemisNbr = 0;

        candidateRoom[1].type = Room.Type.KeyRoom;

        

        Debug.Log($"Shop en ({candidateRoom[0].x}, {candidateRoom[0].y}) avec {candidateRoom[0].sorties.Count} sorties");
        Debug.Log($"Clé en ({candidateRoom[1].x}, {candidateRoom[1].y}) avec {candidateRoom[1].sorties.Count} sorties");
    }
}
