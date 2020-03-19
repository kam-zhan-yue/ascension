using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject room;

    void Start()
    {
        //Instantiate(Resources.Load<GameObject>("down_end/1"), new Vector3(0,0, 0), Quaternion.identity); 
        int[,] route = new int[4, 4];
        PlanRoute(route);
        CheckRoute(route);
        int maxRooms = HighestRoom(route);
        Debug.Log("Highest Room Number is: " + maxRooms);
        CreateRoute(route, maxRooms);
        //for (int i=0; i<100; i++)
        //{
        //    Debug.Log((int)Random.Range(1, 6));
        //}
    }

    void CreateRoute(int[,] route, int maxRooms)
    {
        int search = 1;
        while(search<=maxRooms)
        {
            for(int i=route.GetLength(0)-1; i>-1; i--)
            {
                for(int j=0; j<route.GetLength(1); j++)
                {
                    if(route[i,j] == search)
                    {
                        Debug.Log("Room " + search + " at: " + i + " " + j);
                        CreateRoom(route, i, j, search, maxRooms);
                        //Debug.Log(DetermineRoom(route, i, j, maxRooms));
                        search++;
                    }
                }
            }
        }
    }

    void CreateRoom(int[,] route,int row, int col, int number, int maxRooms)
    {
        int x = 0 + col * 20;
        int y = 0 - row * 10; ;
        //Instantiate(room, new Vector3(x, y, 0), Quaternion.identity);
        if (DetermineRoom(route, row, col, maxRooms) != null)
            Instantiate(Resources.Load<GameObject>(DetermineRoom(route, row, col, maxRooms) + "/" + ((int)Random.Range(1, 6)).ToString()), new Vector3(x, y, 0), Quaternion.identity);


        //Instantiate(Resources.Load<GameObject>(DetermineRoom(route, row, col, maxRooms)), new Vector3(x, y, 0), Quaternion.identity);

    }

    static string DetermineRoom(int[,] route, int row, int col, int maxRooms)
    {
        string roomType = "";
        //Check if starting room
        if (route[row,col] == 1)
        {
            //If at left wall
            if (col == 0)
            {
                //If exit to right
                if (route[row,col + 1] == route[row,col] + 1)
                    roomType = "start_right";
                //If exit to up
                else if (route[row - 1,col] == route[row,col] + 1)
                    roomType = "start_up";
            }
            //If at right wall
            else if (col == route.GetLength(0) - 1)
            {
                //If exit to left
                if (route[row,col - 1] == route[row,col] + 1)
                    roomType = "start_left";
                //If exit to up
                else if (route[row - 1,col] == route[row,col] + 1)
                    roomType = "start_up";
            }
            else
            {
                //If exit to right
                if (route[row,col + 1] == route[row,col] + 1)
                    roomType = "start_right";
                //If exit to left
                else if (route[row,col - 1] == route[row,col] + 1)
                    roomType = "start_left";
                //If exit to up
                else if (route[row - 1,col] == route[row,col] + 1)
                    roomType = "start_up";
            }
        }

        //If not starting room
        else if (route[row,col] < maxRooms)
        {
            //If at left wall
            if (col == 0)
            {
                //If entry from right
                if (route[row,col + 1] == route[row,col] - 1)
                {
                    //If exit to up
                    if (route[row - 1,col] == route[row,col] + 1)
                        roomType = "right_up";
                }

                //If entry from down
                else if (route[row + 1,col] == route[row,col] - 1)
                {
                    //If exit to right
                    if (route[row,col + 1] == route[row,col] + 1)
                        roomType = "down_right";
                    //If exit to up
                    else if (route[row - 1,col] == route[row,col] + 1)
                        roomType = "down_up";
                }
            }

            //If at right wall
            else if (col == route.GetLength(0) - 1)
            {
                //If entry from left
                if (route[row,col - 1] == route[row,col] - 1)
                {
                    //If exit to up
                    if (route[row - 1,col] == route[row,col] + 1)
                        roomType = "left_up";
                }

                //If entry from down
                else if (route[row + 1,col] == route[row,col] - 1)
                {
                    //If exit to left
                    if (route[row,col - 1] == route[row,col] + 1)
                        roomType = "down_left";
                    //If exit to up
                    else if (route[row - 1,col] == route[row,col] + 1)
                        roomType = "down_up";
                }
            }

            //If in the middle
            else
            {
                //If entry from left
                if (route[row,col - 1] == route[row,col] - 1)
                {
                    //If exit to right
                    if (route[row,col + 1] == route[row,col] + 1)
                        roomType = "left_right";
                    //If exit to up
                    else if (route[row - 1,col] == route[row,col] + 1)
                        roomType = "left_up";
                }
                //If entry from right
                else if (route[row,col + 1] == route[row,col] - 1)
                {
                    //If exit to left
                    if (route[row,col - 1] == route[row,col] + 1)
                        roomType = "right_left";
                    //If exit to up
                    else if (route[row - 1,col] == route[row,col] + 1)
                        roomType = "right_up";
                }
                //If entry from down
                else if (route[row + 1,col] == route[row,col] - 1)
                {
                    //If exit to left
                    if (route[row,col - 1] == route[row,col] + 1)
                        roomType = "down_left";
                    //If exit to right
                    else if (route[row,col + 1] == route[row,col] + 1)
                        roomType = "down_right";
                    //If exit to up
                    else if (route[row - 1,col] == route[row,col] + 1)
                        roomType = "down_up";
                }
            }
        }

        //If end room
        else
        {
            //If at left wall
            if (col == 0)
            {
                //If entry from right
                if (route[row,col + 1] == route[row,col] - 1)
                    roomType = "right_end";
                //If entry from down
                else if (route[row + 1,col] == route[row,col] - 1)
                    roomType = "down_end";
            }

            //If at right wall
            else if (col == route.GetLength(0) - 1)
            {
                //If entry from left
                if (route[row,col - 1] == route[row,col] - 1)
                    roomType = "left_end";
                //If entry from down
                else if (route[row + 1,col] == route[row,col] - 1)
                    roomType = "down_end";
            }

            //If in the middle
            else
            {
                //If entry from right
                if (route[row,col + 1] == route[row,col] - 1)
                    roomType = "right_end";
                //If entry from left
                else if (route[row,col - 1] == route[row,col] - 1)
                    roomType = "left_end";
                //If entry from down
                else if (route[row + 1,col] == route[row,col] - 1)
                    roomType = "down_end";
            }
        }
        return roomType;
    }

    static int HighestRoom(int[,] route)
    {
        int max = 0;
        for(int i=0; i<route.GetLength(0); i++)
        {
            for(int j=0; j<route.GetLength(1); j++)
            {
                if (route[i, j] > max)
                    max = route[i, j];
            }
        }
        return max;
    }

    void PlanRoute(int[,] route)
    {
        //Pick the first room
        int order = 1;
        int row = route.GetLength(0) - 1;
        int column = (int)Random.Range(0, route.GetLength(1));
        route[row,column] = order;
        CheckRoute(route);

        int choice = 0;
        bool found = false;
        do
        {
            choice = (int)Random.Range(0, 6);
            switch(choice)
            {
                //1 in 5 chance to go up
                case 1:
                    if (row != 0)
                    {
                        Debug.Log("Go Up");
                        order++;
                        row--;
                        route[row, column] = order;
                        found = true;
                    }
                    else
                        found = false;
                    break;
                //2 in 5 chance to go right
                case 2:
                case 3:
                    if (column != route.GetLength(1) - 1 && route[row, column + 1] == 0)
                    {
                        Debug.Log("Go Right");
                        order++;
                        column++;
                        route[row, column] = order;
                        found = true;
                    }
                    else
                        found = false;
                    break;
                case 4:
                case 5:
                    if (column != 0 && route[row, column - 1] == 0)
                    {
                        Debug.Log("Go Left");
                        order++;
                        column--;
                        route[row, column] = order;
                        found = true;
                    }
                    else
                        found = false;
                    break;
            }

            //If stuck at corner, terminate the loop
            if (row == 0 && column == 0 && route[0,1] != 0)
                order = 15;
            if (row == 0 && column == route.GetLength(1) - 1 && route[0,2] != 0)
                order = 15;
        }
        while (!found || order < 12);
        Debug.Log("Route is complete");
    }

    void CheckRoute(int[,] route)
    {
        string print = "";
        for(int i=0; i<route.GetLength(0); i++)
        {
            for(int j=0; j<route.GetLength(1); j++)
            {
                print += route[i, j].ToString()+" ";
            }
            Debug.Log(print);
            print = "";
        }
    }
}
