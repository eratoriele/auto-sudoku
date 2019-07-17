using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto_Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            // Sudoku in array form

            int[,] sudoku =  new int[,] 
            {
                { 7, 0, 1, 0, 0, 0, 0, 0, 0}, 
                { 0, 0, 0, 6, 0, 0, 5, 0, 0}, 
                { 0, 0, 0, 0, 0, 0, 2, 0, 0},
                { 0, 0, 0, 2, 0, 8, 6, 0, 0},
                { 9, 0, 0, 0, 0, 0, 0, 1, 0}, 
                { 4, 0, 0, 5, 0, 0, 0, 0, 0},
                { 0, 8, 0, 0, 0, 0, 0, 0, 3}, 
                { 0, 0, 0, 0, 9, 0, 0, 7, 0}, 
                { 0, 2, 0, 0, 0, 0, 0, 0, 0}
            };

            printSudoku(sudoku);

            Console.WriteLine("-------------------------\n");


            // ------------------------ Start of the algorithm

            // This algorithm will use brute force.
            // Since it will need to go back and edit prior answers,
            // We need a way to preserve the given numbers.
            // This list will keep which indexes were given, and which are brute forced.
            var fixedValues = new List<Tuple<int, int>>();

            // Add the given numbers to the list.
            for(int i = 0; i < 9; i++){  
                for(int j = 0; j < 9; j++){  
                    if (sudoku[i,j] != 0)
                        fixedValues.Add(new Tuple<int, int>(i, j));
                }
            }

            // Start bruteforcing possible numbers
            // Recursive, because if a feature road is blocked, it can come back
            // And try a different number at that cell


            bruteForceCells(0, 0, sudoku, fixedValues);



        }

        static void printSudoku(int[,] sudoku) {        //Prints the sudoku

            //TODO maybe make it prettier, like the one on top?

            Console.WriteLine("+---+---+---+---+---+---+---+---+---+");
            for(int i = 0; i < 9; i++){  
                for(int j = 0; j < 9; j++){  
                    Console.Write("| " + sudoku[i,j] + " ");  
                }  
                Console.WriteLine("|\n+---+---+---+---+---+---+---+---+---+");
            }
        }

        // This function brute forces the given cell, then calls the next cell.
        // If the next cell fails, it will return here again and give this cell
        // a new value and try again
        static void bruteForceCells(int x, int y, int[,] sudoku, List<Tuple<int,int>> fixedValues) {

            // if this cell is a fixed one
            if (fixedValues.Contains(new Tuple<int, int>(x, y))){
                if (x == 8 && y == 8) { // If the last cell is a fixed one, terminate the process and finish sudoku
                    printSudoku(sudoku);
                    System.Environment.Exit(1);
                }
                else if (y == 8)        // Skip to next line
                    bruteForceCells(x+1, 0, sudoku, fixedValues);
                else                    // Skip to next cell
                    bruteForceCells(x, y+1, sudoku, fixedValues);
            }
            // If it is not a fixed cell
            else {
                // Try all the numbers from 1 to 9
                for (int possibleNum = 1; possibleNum < 10; possibleNum++){
                    // Check the row and coloumn if the possibleNum can be placed
                    var breakFlag = false;
                    for (int i = 0; i < 9; i++){
                        if (sudoku[x,i] == possibleNum || sudoku[i,y] == possibleNum){
                            breakFlag = true;
                            break;
                        }
                    }
                    // Check the 3x3 square if the possibleNum can be placed
                    var groupX = x / 3;
                    var groupY = y / 3;
                    for (int i = groupX*3; i < groupX*3+3; i++){
                        for (int j = groupY*3; j < groupY*3+3; j++){
                            if (sudoku[i,j] == possibleNum) {
                                breakFlag = true;
                                break;
                            }
                        }
                        if (breakFlag)
                            break;
                    }
                    if (!breakFlag) {

                        // No problems so far, put the number to place
                        sudoku[x,y] = possibleNum;

                        // Go to next cell

                        if (x == 8 && y == 8) { // If this is the last cell, terminate the process and finish sudoku
                            printSudoku(sudoku);
                            System.Environment.Exit(1);
                        }
                        else if (y == 8)        // Skip to next line
                            bruteForceCells(x+1, 0, sudoku, fixedValues);
                        else                    // Skip to next cell
                            bruteForceCells(x, y+1, sudoku, fixedValues);

                        
                    }
                }
                // If code reaches this point, it means that a deadend appeared
                // And a new number is necessary on the prior cells
                // reset this cell to 0 before going back, so tghat it doesnt interefere with
                // prior numbers
                sudoku[x,y] = 0;
            }                

        }
    }
}
