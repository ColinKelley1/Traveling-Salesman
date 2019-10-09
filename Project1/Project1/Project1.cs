////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//	Project:        Project1
//	File Name:		Project1.cs
//	Description:    find the fastest delivery route starting at point (0,0) going through all the points and then ending back at (0,0).
//	Course:			CSCI 3230-001
//	Author:			Colin Kelley
//	Created:	    1/19/2018
//  Copywright:     Colin Kelley, 2019
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Project1
{   

    struct Point
    {
        public int x;           //X coord
        public int y;           //Y coord
        public int index;       //Holds index of where originally placed
    }

    class Project1
    {
        //Two objects hold closest and second closest points
        public static Point originFirst;        
        public static Point originSecond;

        //Holds distance of above points to origin
        public static double distance1st;
        public static double distance2nd;

        /// <summary>
        /// Uses other methods to find which permutation path has the lest distance
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            double smallestDistance = 999999999;                  //Distance holds the smallest distance. Initialized as a high number    
            int numPoints = Int32.Parse(Console.ReadLine());      //Reads first line to get number of points
            Int64 numPermutes = 1;                                //Holds number of possible permutations

            Point[] points = new Point[numPoints];               //Declare an array of point structures
            Point[] BestPath = new Point[numPoints - 2];         //Holds the best path possible
            Point[] permArray = new Point[numPoints - 2];        //Holds all points but the two smallest

            String instring;                                //String used in input
            string[] values = new string[2];                //Array used to store values from input

            //This loop gets input from the console and places it in arrays
            for (int i = 0; i < numPoints; i++)
            {
                instring = Console.ReadLine();              // read in point
                values = instring.Split(' ');               // split values

                points[i].index = i;                           //Set the original index
                Int32.TryParse(values[0], out points[i].x);    //insert into x array
                Int32.TryParse(values[1], out points[i].y);    //insert into y array
            }

            //Initialize the stopwatch
            Stopwatch sw = Stopwatch.StartNew();

            findClosePoints(points, ref permArray);     //Find the two closest points and set the array



            int count = numPoints - 2;      //Counter while finding factorial

            //While loop finds numbers factorial
            while(count > 0)
            {
                numPermutes *= count;       //Multiply the factorial
                count--;            //Decrement count
            }

            //Tests distance to see if smaller
            smallerDistance(permArray, ref smallestDistance);

            //If the new distance is smaller then set it
            while(numPermutes - 1 != 0)
            {
                permutation(ref permArray);       //Creates a permutation of the points array
                
                //If the first index is larger than the last then the permute has already been made
                if (!(permArray[0].index > permArray[permArray.Length - 1].index))
                {
                    //Gets distance of permutation
                    if (smallerDistance(permArray, ref smallestDistance))
                    {
                        //Itterates through every point
                        for (int i = 0; i < BestPath.Length;i++)
                        {
                            //Copies the array to the other
                            BestPath[i].index = permArray[i].index;
                            BestPath[i].x = permArray[i].x;
                            BestPath[i].y = permArray[i].y;
                        }
                    }
                }

                numPermutes--;      //Decrement numPermutes
            }

            //Gives more accurate numbers than double
            float floatSmall = (float)smallestDistance;

            //Calculates as double     
            Console.WriteLine("Total Distance: " + (floatSmall.ToString("0.00")));

            //Stop time and show answer
            sw.Stop();
            Console.WriteLine("Time used: {0} Seconds", sw.Elapsed.TotalMilliseconds / 1000);

            Console.WriteLine("Optimal Route: ");           //Display prompt
            Console.WriteLine("0");                         //Display origin
            Console.WriteLine(originFirst.index + 1);       //Display first point

            for (int i = 0; i < BestPath.Length; i++)       //Display path
                Console.WriteLine(BestPath[i].index + 1);

            Console.WriteLine(originSecond.index + 1);                //Display last point
        }


        /// <summary>
        /// Finds the two closest points to the origin
        /// </summary>
        /// <param name="pArray"></param>
        /// <param name="permArray"></param>
        static void findClosePoints(Point[] pArray, ref Point[] permArray)
        {
            double distance;               //Holds distance of points
            double closest = 99999999;     //Holds closest distance
            int noCopy = -1;               //Holds index of point not to be copied
            int noCopy01 = -1;             //Index of second point that does not need to be copied
            int permCount = 0;             //Indexes through permArray when copying

            //For loop finds the closest point
            for (int i = 0; i < pArray.Length; i++)
            {
                //Get current distance between the two points
                distance = Math.Sqrt((pArray[i].x * pArray[i].x) + (pArray[i].y * pArray[i].y));

                //If it is the closest then set it
                if (distance < closest)
                {
                    closest = distance;         //Set closest distance
                    noCopy = pArray[i].index;   //Set the index
                    distance1st = distance;     //Set the point distance
                }
            }


            closest = 999999999;     //reset the closest distance
            //For loop finds the second closest point
            for (int i = 0; i < pArray.Length; i++)
            {
                //If the index is not already the closest
                if (pArray[i].index != noCopy)
                {
                    //Get current distance between the two points
                    distance = Math.Sqrt((pArray[i].x * pArray[i].x) + (pArray[i].y * pArray[i].y));

                    //If it is the closest then set it
                    if (distance < closest)
                    {
                        closest = distance;             //Set the closest distance
                        noCopy01 = pArray[i].index;     //Set the second index
                        distance2nd = distance;         //Set point distance
                    }
                }
            }

            //For loop copies array without the closest points
            for (int i = 0; i < pArray.Length; i++)
            {
                //Skip the copy if it is either point
                if (!(pArray[i].index == noCopy || pArray[i].index == noCopy01))
                {
                    //Copies the array to the other
                    permArray[permCount].index = pArray[i].index;
                    permArray[permCount].x = pArray[i].x;
                    permArray[permCount].y = pArray[i].y;

                    permCount++;       //Increment the counter
                }
            }

            //Block copies data into originFirst
            originFirst.index = noCopy;
            originFirst.x = pArray[noCopy].x;
            originFirst.y = pArray[noCopy].y;

            //Block copies data into origin last
            originSecond.index = noCopy01;
            originSecond.x = pArray[noCopy01].x;
            originSecond.y = pArray[noCopy01].y;
        }



        /// <summary>
        /// Gets array and smallest. Checks to see if smaller. If it is then it sets smallest and returns true
        /// </summary>
        /// <param name="pointsArray"></param>
        /// <param name="smallest"></param>
        /// <returns>True if smaller, false otherwise</returns>
        static bool smallerDistance(Point[] pointsArray, ref double smallest)
        {
            double currentDistance = 0;           //Holds current distance
            double totalDistance = 0;             //Holds the total distance between all the points
            int length = pointsArray.Length;      //Gets length once so you dont have to later       

            totalDistance += distance1st;         //Add the distance from the origin to first point

            //Calculates the distance from the first point to the second
            currentDistance = Math.Sqrt(((originFirst.x - pointsArray[0].x) * (originFirst.x - pointsArray[0].x)) + ((originFirst.y - pointsArray[0].y) * (originFirst.y - pointsArray[0].y)));
            totalDistance += currentDistance;   //Add to the total

            //Calculate the permmutation's distance
            for (int i = 0; i <= length - 2; i++)
            {
                //Get current distance between the two points
                currentDistance = Math.Sqrt(((pointsArray[i + 1].x - pointsArray[i].x) * (pointsArray[i + 1].x - pointsArray[i].x)) + ((pointsArray[i + 1].y - pointsArray[i].y) * (pointsArray[i + 1].y - pointsArray[i].y)));

                totalDistance += currentDistance;       //Add the current distance to the total

                //If the total is ever greater then we dont need to calculate any more
                if(totalDistance > smallest)
                {
                    return false;
                }
            }

            //Calculates the last array point to last point
            currentDistance = Math.Sqrt(((originSecond.x - pointsArray[length - 1].x) * (originSecond.x - pointsArray[length - 1].x)) + ((originSecond.y - pointsArray[length - 1].y) * (originSecond.y - pointsArray[length - 1].y)));

            totalDistance += currentDistance;   //Add to the total

            totalDistance += distance2nd;       //Add last points distance

            //If the new distance is smaller then set it to be new
            if (totalDistance < smallest)
            {
                smallest = totalDistance;
                return true;
            }
            //Otherwise return false
            return false;
            

        }

        /// <summary>
        /// Swaps the two points provided
        /// </summary>
        /// <param name="pointArray"></param>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        static void SwapPoints (ref Point[] pointArray, int index1, int index2)
        {
            Point Store = pointArray[index1];         //Store the point
            pointArray[index1] = pointArray[index2];  //Put swap2 in 1s place
            pointArray[index2] = Store;               //Put swap1 in 2s place
        }


        /// <summary>
        /// Takes in an array of points and makes the next permutation
        /// </summary>
        /// <param name="points"></param>
        /// <returns>New array of points</returns>
        static void permutation(ref Point[] points)
        {
            

            int swap1 = -1;        //Variable holds first index to be swapped
            int swap2 = -1;        //Variable holds second index to be swapped
            
            //Find the next decrease
            for (int i = points.Length - 2; i > -1; i--)
            {
                //See if the next point is greater
                if (points[i + 1].index > points[i].index)
                {
                    //Set the swap index
                    swap1 = i;
                    break; 
                }
            }

            //Starts from the back in order to find next swap
            for (int i = points.Length - 1; i > -1; i--)
            {
                //See if the next point is greater
                if (points[swap1].index < points[i].index)
                {
                    //Set the swap index
                    swap2 = i;
                    break;
                }
            }

            int lastPoint = points.Length - 1;                                      //Get the index of the last point

            SwapPoints(ref points, swap1, swap2);       //Swaps the two points

            //Swap previous numbers if needed in order to sort
            for (int i = swap1 + 1; i < lastPoint; i++)
            {
                SwapPoints(ref points, i, lastPoint);   //Swap the two points
                lastPoint--;                                                        //Decrement lastPoint
            }

            return;      //Return the new array
        }
    }
}
