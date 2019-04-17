using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTetris
{
    class Figure
    {

        public List <Point> points  = new List<Point>();
        public List<Point> lowestPoints;
        Point mainPoint;
        int type;
        int rotationCout=1;
        public Figure(int type, int x, int y)
        {
            this.type = type;
            mainPoint = new Point(x, y);
            SetPoints(type, x, y);
        }

        public void Refresh(int x, int y)
        {
            foreach(Point point in points)
            {
                point.y += y;
                point.x += x;
            }
            lowestPoints = FindLowestPoints();
        }
        public void Refresh(List<Point> newPoints)
        {
            points = newPoints;
            lowestPoints = FindLowestPoints();
        }
        public List<Point> FigureRotate() {
            int x = points.ElementAt(0).x;
            int y = points.ElementAt(0).y;

            Figure tmp = new Figure(type, 2, 2);
            
           
            int[,] arrForRotate = new int[5, 5];
            int counter=3;

            while (counter >= 0)
            {
                arrForRotate[tmp.points.ElementAt(counter).x, tmp.points.ElementAt(counter).y]=1;
                counter--;
            }
            int n = 5;
            for (int e = 0; e < rotationCout; e++)
            {

                for (int j = 0; j < n/2; ++j)
                {
                    for (int i = j; i < n-j-1; ++i)
                    {
                        int tmpEl = arrForRotate[i,j];
                        arrForRotate[i,j] = arrForRotate[n - j - 1, i];
                        arrForRotate[n - j - 1, i] = arrForRotate[n - i - 1, n - j - 1];
                        arrForRotate[n - i - 1, n - j - 1] = arrForRotate[j, n - i - 1];
                        arrForRotate[j, n - i - 1] = tmpEl;

                    }
                }
            }

            rotationCout++;
            if (rotationCout > 4)
                rotationCout = 1;

            return ArrToPoints(arrForRotate, x, y);

        }


        void SetPoints(int type, int x, int y) {
            switch (type)
            {
                case 0://I
                    points.Add(mainPoint);
                    points.Add(new Point(x, y + 1));
                    points.Add(new Point(x, y - 1));
                    points.Add(new Point(x, y - 2));
                    break;
                case 1://O
                    points.Add(mainPoint);
                    points.Add(new Point(x, y - 1));
                    points.Add(new Point(x - 1, y - 1));
                    points.Add(new Point(x - 1, y));
                    break;

                case 2://L
                    points.Add(mainPoint);
                    points.Add(new Point(x, y - 1));
                    points.Add(new Point(x - 1, y - 1));
                    points.Add(new Point(x , y + 1));
                    break;
                case 3://!L
                    points.Add(mainPoint);
                    points.Add(new Point(x, y - 1));
                    points.Add(new Point(x + 1, y - 1));
                    points.Add(new Point(x, y + 1));
                    break;
                case 4://T
                    points.Add(mainPoint);
                    points.Add(new Point(x, y - 1));
                    points.Add(new Point(x - 1, y ));
                    points.Add(new Point(x, y + 1));
                    break;
                case 5://S
                    points.Add(mainPoint);
                    points.Add(new Point(x + 1, y ));
                    points.Add(new Point(x + 1, y + 1));
                    points.Add(new Point(x, y - 1));
                    break;
                case 6://!S
                    points.Add(mainPoint);
                    points.Add(new Point(x - 1, y));
                    points.Add(new Point(x - 1, y + 1));
                    points.Add(new Point(x, y - 1));
                    break;
            }
            lowestPoints = FindLowestPoints();
        }
        List<Point> ArrToPoints(int [,] arr, int x, int y)
        {
           List<Point> resultPoints = new List<Point>();

            resultPoints.Add(new Point(x, y));
            arr[2, 2] = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {

                    if (arr[j, i] == 1)
                    {
                        Point point = new Point(j + x - 2, i + y - 2);
                        if (point.x < 10 && point.x >= 0 && point.y < 20 && point.y >= 0)
                            resultPoints.Add(point);
                        else
                            return points;
                    }
                }
            }
            return resultPoints;
        }

        public List<Point> FindLowestPoints()
        {

            List<Point> result = new List<Point>();

            Dictionary<int, Point> buffer = new Dictionary<int, Point>();

            Point lowestInColumn = new Point(0, 0);
            foreach (Point secondTmp in points)
            {
                lowestInColumn.x = secondTmp.x;
                foreach (Point tmp in points)
                {
                    if (lowestInColumn.x == tmp.x && lowestInColumn.y < tmp.y)
                        lowestInColumn = tmp;
                }
                buffer[lowestInColumn.x] = lowestInColumn;

                lowestInColumn = new Point(0, 0);
            }
            foreach (Point secondTmp in buffer.Values)
            {
                result.Add(secondTmp);
            }
            return result;
        }
    }
    class GameField
    {
        public int score;
        public int[,] field;
        public Figure сurrentFigure;
        GameField smallField;
        public GameField(int sizeX, int sizeY)
        {
            if (sizeY > 5)
                сurrentFigure = new Figure(new Random().Next() % 7, 5, 2);
            else
                сurrentFigure = new Figure(new Random().Next() % 7, 2, 2);
            field = new int[sizeX, sizeY];
        }
        public GameField(int sizeX, int sizeY,GameField smallField) {
            if(sizeY>5)
            сurrentFigure = new Figure(new Random().Next() % 7, 5, 2);
            else
                сurrentFigure = new Figure(new Random().Next() % 7, 2, 2);
            this.smallField = smallField;
            field = new int[sizeX, sizeY];
        }
        public void FigureRotate() {
            List<Point> tmp;
            if (!CheckSideFigure(0, tmp = сurrentFigure.FigureRotate()))
                сurrentFigure.Refresh(tmp);
        }
        public void FigureStepDown() {
            if (!CheckY(19)&&!CheckLower())
            {
                сurrentFigure.Refresh(0, 1);
            }
            else
            {
                SetFigure();
                score += 5;
                if (field.Length > 5)
                {
                    Console.WriteLine(field.Length);
                    smallField.сurrentFigure.Refresh(3, 0);
                    сurrentFigure = smallField.сurrentFigure;
                    smallField.сurrentFigure= new Figure(new Random().Next() % 7, 2, 2);
                }
            }
        }
        
        public void FigureMove(int direction)
        {
            if (direction > 0 && !CheckX(9))
            {
                direction = 1;
            }
            else if (direction <= 0&&!CheckX(0))
            {
                direction = -1;
            }
            else
                direction = 0;
            if(!CheckSideFigure(direction, сurrentFigure.points))
            сurrentFigure.Refresh(direction, 0);
           }

        bool CheckSideFigure(int direction, List<Point> points)
        {
            foreach (Point tmpPoint in points) {
                if (field[tmpPoint.x + direction, tmpPoint.y] == 1)
                    return true;
            }
                return false;
        }
        bool CheckX(int column)
        {
            foreach (Point tmpPoint in сurrentFigure.points)
                if (tmpPoint.x == column)
                    return true;
            return false;
        }
        public bool LineFull(int y)
        {
            for (int x = 0; x < 10; x++)
            {
                if (field[x, y] != 1)
                    return false;
            }
            score += 100;
            return true;
        }
        public void MoveLinesDown(int fromRaw)
        {
            for(int y = fromRaw; y > 0; y--)
            {
                for(int x = 0; x < 10; x++)
                {
                    field[x, y] = field[x, y - 1];
                }
            }
        }
        public void DropFigure() {
            while (!CheckY(19) && !CheckLower())
            {
                сurrentFigure.Refresh(0, 1);
            }

        }
        bool CheckY(int raw)
        {
            foreach (Point tmpPoint in сurrentFigure.points)
                if (tmpPoint.y == raw)
                    return true;
            return false;
        }
        public bool CheckLower() {
            foreach(Point tmp in сurrentFigure.lowestPoints)
            {
                if (field[tmp.x, tmp.y + 1] == 1)
                    return true;
            }
            return false;
        }
        public bool SetFigure()
        {
            foreach (Point tmpPoint in сurrentFigure.points)
            {
                field[tmpPoint.x, tmpPoint.y] = 1;
            }
            return true;
        }
        public bool CheckLose() {
            for (int x = 0; x < 10; x++)
            {
                if (field[x, 2] == 1)
                    return true;
            }
            return false;
        }
    }
    public class Point
    {
        public int x, y;
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
