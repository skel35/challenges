using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class Player
{
    public struct Point
    {
        public long x, y;
        public Point(int x, int y) { this.x = x; this.y = y; }
        public Point(long x, long y) { this.x = x; this.y = y; }
        public Point(PointD p) { x = (int)Math.Round(p.x); y = (int)Math.Round(p.y); }
        public void Print()
        {
            Console.Write("{0} {1}", x, y);
        }
        public void PrintErr()
        {
            Console.Error.Write("({0} {1})", x, y);
        }
        public long ModuleSqr()
        {
            return x * x + y * y;
        }
        public double Module()
        {
            return Math.Sqrt(x * x + y * y);
        }
        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.x + p2.x, p1.y + p2.y);
        }
        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.x - p2.x, p1.y - p2.y);
        }
        public static Point operator -(Point p1)
        {
            return new Point(-p1.x, -p1.y);
        }
        public static Point operator *(Point p1, int c)
        {
            return new Point((c * p1.x), (c * p1.y));
        }
        public static Point operator /(Point p1, double c)
        {
            return new Point((int)(p1.x / (double)c), (int)(p1.y / (double)c));
        }
        public static PointD operator +(Point p1, PointD p2)
        {
            PointD res; res.x = p1.x + p2.x; res.y = p1.y + p2.y;
            return res;
        }
        public static PointD operator -(Point p1, PointD p2)
        {
            PointD res; res.x = p1.x - p2.x; res.y = p1.y - p2.y;
            return res;
        }
        public static PointD operator *(Point p1, double d)
        {
            PointD res; res.x = p1.x * d; res.y = p1.y * d;
            return res;
        }
        public static double operator *(Point p1, PointD p2)
        {
            return p1.x * p2.x + p1.y * p2.y;
        }
    }
    public struct PointD
    {
        public double x, y;
        public static PointD operator *(PointD p1, double d)
        {
            PointD res; res.x = p1.x * d; res.y = p1.y * d;
            return res;
        }
    }
    public class GameMove
    {
        public Point Destination { get; set; }
        public int Thrust { get; set; }
        public bool Shield { get; set; }
        public string msg;
        public GameMove(Point destination, int thrust, bool shield = false, string message = "")
        {
            Destination = destination; Thrust = thrust; Shield = shield; msg = message;
        }
        public GameMove(Point destination, bool shield, string message = "")
        {
            Destination = destination; Thrust = 0;
            Shield = shield; // true
            msg = message;
        }
        public void AddDegree(int degree)
        {
            double radians = degree * 180.0 / Math.PI;
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);
            int newX = (int)Math.Round(Destination.x * cos + Destination.y * sin);
            int newY = (int)Math.Round(Destination.y * cos - Destination.x * sin);
            Destination = new Point(newX, newY);
        }
        public void Print()
        {
            Destination.Print();
            if (Shield) Console.Write(" SHIELD");
            else Console.Write(" {0}", Thrust);
            //if (msg.Length > 1) Console.WriteLine(" " + msg);
            //else
                Console.WriteLine();
        }
        static public void Copy(GameMove[] from, GameMove[] to, int length)
        {
            for (int i = 0; i < length; i++)
            {
                to[i] = new GameMove(from[i].Destination, from[i].Thrust, from[i].Shield, from[i].msg);
                //to[i].Destination = from[i].Destination;
                //to[i].Thrust = from[i].Thrust;
                //to[i].Shield = from[i].Shield;
            }
        }
        static public GameMove MoveWithShield(GameMove move)
        {
            return new GameMove(move.Destination, true);
        }
        static public List<GameMove[]> GenerateMoves(GameMove[] moves, int runner)
        {
            List<GameMove[]> list = new List<GameMove[]>();
            //int j1 = 0;
            int step = 30;
            for (int j1 = -step; j1 <= step; j1 += step)
            {
                if (moves[runner].Thrust + j1 <= 200 && moves[runner].Thrust + j1 >= 0)
                {
                    GameMove[] moves2 = new GameMove[4];
                    Copy(moves, moves2, 2);
                    //Array.Copy(moves, moves2, 2);
                    moves2[runner].Thrust += j1;
                    for (int degree1 = -18; degree1 <= 18; degree1 += 6)
                    {
                        GameMove[] moves3 = new GameMove[4];
                        Copy(moves2, moves3, 2);
                        moves3[runner].AddDegree(degree1);
                        list.Add(moves3);
                    }
                }
            }
            return list;
        }
        static public List<GameMove[]> GenerateMoves(GameMove[] moves)
        {
            List<GameMove[]> list = new List<GameMove[]>();
            //int j1 = 0;
            int step = 30;
            for (int j1 = -step; j1 <= step; j1 += step)
            {
                //int j2 = 0;
                for (int j2 = -step; j2 <= step; j2 += step)
                {
                    if (moves[0].Thrust + j1 <= 200 && moves[0].Thrust + j1 >= 0)
                    {
                        if (moves[1].Thrust + j2 <= 200 && moves[1].Thrust + j2 >= 0)
                        {
                            GameMove[] moves2 = new GameMove[4];
                            Copy(moves, moves2, 2);
                            //Array.Copy(moves, moves2, 2);
                            moves2[0].Thrust += j1;
                            moves2[1].Thrust += j2;
                            for (int degree1 = -18; degree1 <= 18; degree1 += 6)
                            {
                                for (int degree2 = -18; degree2 <= 18; degree2 += 6)
                                {
                                    GameMove[] moves3 = new GameMove[4];
                                    Copy(moves2, moves3, 2);
                                    moves3[0].AddDegree(degree1);
                                    moves3[1].AddDegree(degree2);
                                    list.Add(moves3);
                                }
                            }

                        }
                    }
                }
            }

            return list;
        }

    }
    public class Pod
    {
        public Point p;
        public Point v;
        public long X { get { return p.x; } set { p.x = value; } }
        public long Y { get { return p.y; } set { p.y = value; } }
        public long VX { get { return v.x; } set { v.x = value; } }
        public long VY { get { return v.y; } set { v.y = value; } }
        public int Angle { get; set; }
        public int NextCP { get; set; }  // next check point Id
        public int Lap { get; set; }
        public int WaitToThrust { get; set; }
        public bool braking;
        const int Radius = 400;
        const double Friction = 0.85;
        public bool CrossedCP(int margin = 0)
        {
            return Dist(p, cp[NextCP]) < (600 - margin) * (600 - margin);
        }
        public double Utility()
        {
            double res = 0;
            res += (Lap - 1) * 10000 + ((NextCP + C - 1) % C) * (10000.0 / C);
            double cp_progress = 600 - ((p - cp[NextCP]).ModuleSqr() * 600.0 /
                (cp[(NextCP + C - 1) % C] - cp[NextCP]).ModuleSqr());
            if (cp_progress < 0) cp_progress /= 3.0;
            res += cp_progress;
            //if (Lap == laps && NextCP == 1) res += 5000;
            return res;
        }
        static public void UpdateAngle(Pod[] pod, GameMove[] move)
        {
            for (int i = 0; i < 4; i++)
            {
                double angle1 = SpeedAngle(move[i].Destination - pod[i].p);
                double angleDiff = angle1 - pod[i].Angle;
                if (angleDiff > 180) angleDiff -= 360;
                else if (angleDiff < -180) angleDiff += 360;

                if (angleDiff > 18) angleDiff = 18;
                else if (angleDiff < -18) angleDiff = -18;
                pod[i].Angle = (int)Math.Round(pod[i].Angle + angleDiff) % 360;
                if (pod[i].Angle < 0) pod[i].Angle += 360;
            }
            //for (int i = 0; i < 2; i++)
            //{
            //    Console.Error.WriteLine("New angle = {0}", pod[i].Angle);
            //}
        }
        static public void UpdatePos(Pod[] pod, GameMove[] move)
        {
            for (int i = 0; i < 4; i++)
            {
                if (move[i].Shield) pod[i].WaitToThrust = 3;
                else pod[i].WaitToThrust--;
                if (pod[i].WaitToThrust > 0) move[i].Thrust = 0;
            }
            // 1. Target:
            UpdateAngle(pod, move);
            // 2. Thrust:
            PointD[] dV = new PointD[4];
            for (int i = 0; i < 4; i++)
            {
                // possible optimization - pump Sin values for all degrees 0..90 into array
                // + transformation to get all 0..360
                dV[i] = GetVector(pod[i].Angle, move[i].Thrust);
                pod[i].v += new Point(dV[i]);
            }
            bool[] collided = new bool[4];
            // 3. Movement (+collision):
            // TODO: multiple simultaneous collisions case
            for (int i = 0; i < 3; i++)
            {
                for (int j = i + 1; j < 4; j++)
                {
                    Point pos1 = pod[i].p + (pod[i].v - pod[j].v);
                    long denom = Dist(pod[i].p, pos1);
                    long b = (pod[i].VX - pod[j].VX) * (pod[i].X - pod[j].X) +
                        (pod[i].VY - pod[j].VY) * (pod[i].Y - pod[j].Y);
                    double sD = Math.Sqrt(b * b - denom * (Dist(pod[i].p, pod[j].p) - 640000));
                    double alpha1 = (-b + sD) / denom;
                    double alpha2 = (-b - sD) / denom;
                    bool bam = true;
                    if (alpha1 > 0 && alpha1 < 1)
                    {
                        if (alpha2 > 0 && alpha2 < alpha1)
                        {
                            // alpha2 - point of collision
                            ProcessCollision(ref pod[i], ref pod[j], move[i], move[j], alpha2);
                        }
                        else
                        {
                            // alpha1 - point of collision
                            ProcessCollision(ref pod[i], ref pod[j], move[i], move[j], alpha1);
                        }
                    }
                    else if (alpha2 > 0 && alpha2 < 1)
                    {
                        // alpha2 - point of collision
                        ProcessCollision(ref pod[i], ref pod[j], move[i], move[j], alpha2);
                    }
                    else
                    {
                        bam = false;
                    }
                    //no collision

                    if (bam)
                    {
                        collided[i] = true; collided[j] = true;
                        //Console.Error.WriteLine("pod {0} and pod {1} collided; bf speed 1:({2},{3}), 2:({4},{5})",
                        //    i, j, pod[i].VX, pod[i].VY, pod[j].VX, pod[j].VY);
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (!collided[i])
                {
                    pod[i].p += pod[i].v;
                }

                if (pod[i].CrossedCP(100 * ((3 - i) / 2)))
                {
                    if (pod[i].NextCP == 0)
                    {
                        pod[i].Lap++;
                        //if (pod[i].Lap > laps)
                        //{
                        //    // game over
                        //    // pod[i] is winner
                        //}
                    }
                    pod[i].NextCP = (pod[i].NextCP + 1) % C;
                }
            }

            // 4. Friction:
            for (int i = 0; i < 4; i++)
            {
                pod[i].VX = (int)Math.Truncate(pod[i].VX * Friction);
                pod[i].VY = (int)Math.Truncate(pod[i].VY * Friction);
            }

        }
        static public void ProcessCollision(ref Pod pod1, ref Pod pod2, GameMove move1, GameMove move2, double alpha)
        {
            //Console.Error.WriteLine("pre: speed1 ({0},{1}), speed2 ({2},{3})", pod1.VX, pod1.VY, pod2.VX, pod2.VY);
            pod1.p += new Point(pod1.v * alpha);
            pod2.p += new Point(pod2.v * alpha);
            //double d = Math.Sqrt(Math.Pow(pod1.X - pod2.X, 2) + Math.Pow(pod1.Y - pod2.Y, 2));
            double d = (pod1.p - pod2.p).Module();
            PointD n = (pod2.p - pod1.p) * (1.0 / d);
            int m1 = 1 + (move1.Shield ? 9 : 0); int m2 = 1 + (move2.Shield ? 9 : 0);
            double p = 2.0 * (pod1.v * n - pod2.v * n) / (m1 + m2);
            PointD deltaV1 = n * (p * m2);
            PointD deltaV2 = n * (p * m1);
            double deltaImpulse = Math.Sqrt(deltaV1.x * deltaV1.x + deltaV1.y * deltaV1.y);
            if (deltaImpulse < 240)
            {
                double beta = ((240 - deltaImpulse) / 2.0) / deltaImpulse;
                p += p * beta;
            }
            PointD w1 = pod1.v - deltaV1;
            PointD w2 = pod2.v + deltaV2;

            pod1.v = new Point(w1);
            pod2.v = new Point(w2);

            pod1.p += new Point(pod1.v * (1 - alpha));
            pod2.p += new Point(pod2.v * (1 - alpha));
        }
        static public bool Collision(Pod pod1, Pod pod2)
        {
            var dX = pod1.X - pod2.X; var dY = pod1.Y - pod2.Y;
            return dX * dX + dY * dY <= 640000; // 640000 = (2*radius)^2
        }

        public void Update(string input)
        {
            string[] inputs = input.Split(' ');
            if (NextCP == 0 && int.Parse(inputs[5]) == 1)
            {
                Lap++;
            }
            Update(int.Parse(inputs[0]), int.Parse(inputs[1]), int.Parse(inputs[2]),
                    int.Parse(inputs[3]), int.Parse(inputs[4]), int.Parse(inputs[5]));
        }
        public void Update(int x, int y, int vx, int vy, int angle, int nextCP)
        {
            p = new Point(x, y);
            v = new Point(vx, vy);
            Angle = angle;
            NextCP = nextCP;
        }
        public Pod() { }
        public Pod(int x, int y, int vx, int vy, int angle, int nextCP)
        {
            p = new Point(x, y);
            v = new Point(vx, vy);
            Angle = angle;
            NextCP = nextCP;
            Lap = 0; WaitToThrust = 0;
        }
        public Pod(long x, long y, long vx, long vy, int angle, int nextCP)
        {
            p = new Point(x, y);
            v = new Point(vx, vy);
            Angle = angle;
            NextCP = nextCP;
            Lap = 0; WaitToThrust = 0;
        }

        public Pod(Pod oldPod) : this(oldPod.X, oldPod.Y, oldPod.VX, oldPod.VY, oldPod.Angle, oldPod.NextCP)
        {
            Lap = oldPod.Lap; WaitToThrust = oldPod.WaitToThrust;
        }
    }

    class Situation
    {
        public Pod[] pods;
        public Situation(Pod[] myPod, Pod[] oppPod)
        {
            pods = new Pod[] { myPod[0], myPod[1], oppPod[0], oppPod[1] };
        }
        public Situation(Pod myPod1, Pod myPod2, Pod oppPod1, Pod oppPod2)
        {
            pods = new Pod[] { myPod1, myPod2, oppPod1, oppPod2 };
        }
        public Situation(Pod[] pod)
        {
            pods = pod;
        }
        public double UtilityHR()
        {
            double res = 0;
            double u1 = pods[0].Utility();
            double u2 = pods[1].Utility();
            int runner = (u1 > u2) ? 0 : 1;
            res += u1;
            res += u2;
            res += 3 * Math.Max(u1, u2);
            res -= 0.8 * Math.Min(u1, u2);

            res += pod[runner].v.Module() / 800.0;

            res -= 1.3 * Math.Max(pods[2].Utility(), pods[3].Utility());
            //res -= pods[3].Utility();

            return res;
        }
        public double Utility()
        {
            double res = 0;
            double u1 = pods[0].Utility();
            double u2 = pods[1].Utility();
            int runner = (u1 > u2)? 0 : 1;
            res += u1;
            res += u2;
            res += 3 * Math.Max(u1, u2);
            res -= 0.5 * Math.Min(u1, u2);

            res += pod[runner].v.Module() / 800.0;

            res -= 1.3 * Math.Max(pods[2].Utility(), pods[3].Utility());
            //res -= pods[3].Utility();

            return res;
        }
        //public double UtilityRacer()
        //{
        //    double res = 0;
        //    double u1 = pods[0].Utility();
        //    double u2 = pods[1].Utility();
        //    res += u1;
        //    res += u2;
        //    res += 4 * Math.Max(u1, u2);
        //    //res -= 0.2 * Math.Min(u1, u2);

        //    res -= 0.3 * Math.Max(pods[2].Utility(), pods[3].Utility());
        //    //res -= pods[3].Utility();

        //    return res;
        //}
        //public double UtilityHitter()
        //{
        //    double res = 0;
        //    double u1 = pods[0].Utility();
        //    double u2 = pods[1].Utility();
        //    res += u1;
        //    res += u2;
        //    res += 0.9 * Math.Max(u1, u2);
        //    //res -= 0.2 * Math.Min(u1, u2);

        //    res -= 3.0 * Math.Max(pods[2].Utility(), pods[3].Utility());
        //    //res -= pods[3].Utility();

        //    return res;
        //}
        public void Print()
        {
            for (int i = 0; i < 2; i++)
            {
                Console.Error.Write("Pos ");
                pods[i].p.PrintErr();
                Console.Error.Write("Speed ");
                pods[i].v.PrintErr();
                Console.Error.Write(" Utility: {0}", Utility());
                //Console.Error.Write(" Opp: Pos ");
                //pods[i + 2].p.PrintErr();
                //Console.Error.Write("Speed ");
                //pods[i + 2].v.PrintErr();
                Console.Error.WriteLine();
            }
        }
        static public void PrintCurrent()
        {
            for (int i = 0; i < 2; i++)
            {
                Console.Error.Write("Pos ");
                pod[i].p.PrintErr();
                Console.Error.Write("Speed ");
                pod[i].v.PrintErr();
                Console.Error.Write("; bf: ({0},{1})", pod[i].VX / 0.85, pod[i].VY / 0.85);
                //Console.Error.Write(" Opp: Pos ");
                //opp[i].p.PrintErr();
                //Console.Error.Write("Speed ");
                //opp[i].v.PrintErr();
                Console.Error.WriteLine();
            }
        }
    }

    class Simulator
    {
        public int OppDepth;
        public int SimpleSimDepth;
        public Simulator(int oppdepth = 1, int simplesimdepth = 5)
        {
            OppDepth = oppdepth;
            SimpleSimDepth = simplesimdepth;
        }
        public Situation SimulateHR(Pod pod1, Pod pod2, Pod opp1, Pod opp2, GameMove[] moves, int runner)
        {
            var strategy = new SimpleStrategy();

            Console.SetError(StreamWriter.Null);
            var fastStrat = new FastStrategy(OppDepth);

            moves[2] = strategy.GetMove(opp1);
            moves[3] = strategy.GetMove(opp2);

            var standardError = new StreamWriter(Console.OpenStandardError());
            standardError.AutoFlush = true;
            Console.SetError(standardError);

            Pod[] sPod = new Pod[4];
            sPod[0] = new Pod(pod1); sPod[1] = new Pod(pod2);
            sPod[2] = new Pod(opp1); sPod[3] = new Pod(opp2);
            Pod.UpdatePos(sPod, moves);

            GameMove[] moves2 = new GameMove[4];

            GameMove.Copy(moves, moves2, 4);

            for (int i = 0; i < SimpleSimDepth; i++)
            {
                moves2[runner] = strategy.GetMove(sPod[runner]);
                moves2[runner ^ 1] = strategy.GetMoveHit(sPod, runner ^ 1);

                moves2[2] = strategy.GetMove(sPod[2]);
                moves2[3] = strategy.GetMove(sPod[3]);
                Pod.UpdatePos(sPod, moves2);
            }

            return new Situation(sPod);
        }
        public Situation Simulate(Pod pod1, Pod pod2, Pod opp1, Pod opp2, GameMove[] moves, bool Opp = false)
        {
            var strategy = new SimpleStrategy();

            Console.SetError(StreamWriter.Null);
            var fastStrat = new FastStrategy(OppDepth);

            if (!Opp)
            {
                moves[2] = strategy.GetMove(opp1);
                moves[3] = strategy.GetMove(opp2);
            }
            else
            {
                GameMove[] oppMoves = fastStrat.GetMoves(new Pod[] { opp1, opp2 }, new Pod[] { pod1, pod2 });
                moves[2] = oppMoves[0];
                moves[3] = oppMoves[1];
            }

            var standardError = new StreamWriter(Console.OpenStandardError());
            standardError.AutoFlush = true;
            Console.SetError(standardError);

            Pod[] sPod = new Pod[4];
            sPod[0] = new Pod(pod1); sPod[1] = new Pod(pod2);
            sPod[2] = new Pod(opp1); sPod[3] = new Pod(opp2);
            Pod.UpdatePos(sPod, moves);

            GameMove[] moves2 = new GameMove[4];

            GameMove.Copy(moves, moves2, 4);

            for (int i = 0; i < SimpleSimDepth; i++)
            {
                moves2[0] = strategy.GetMove(sPod[0]);
                moves2[1] = strategy.GetMove(sPod[1]);
                moves2[2] = strategy.GetMove(sPod[2]);
                moves2[3] = strategy.GetMove(sPod[3]);
                Pod.UpdatePos(sPod, moves2);
            }

            return new Situation(sPod);
        }
    }
    interface IStrategy
    {
        //GameMove GetMove(Pod pod, Pod friend);
        GameMove[] GetMoves(Pod[] pod, Pod[] opp);
        // leaving opp's pods as static variables
    }
    class HitStrategy : IStrategy
    {
        public int Depth;
        public bool Opp = false;
        public HitStrategy(int depth = 2)
        {
            Depth = depth;
        }

        public GameMove[] GetMoves(Pod[] pod, Pod[] opp)
        {
            GameMove[] moves = new GameMove[4];
            //moves = GetMoves(pod, opp, Depth);
            int runner = (pod[0].Utility() > pod[1].Utility()) ? 0 : 1;
            moves[runner] = (new SimpleStrategy()).GetMove(pod[runner]);
            moves[runner ^ 1] = (new SimpleStrategy()).GetMoveHit(pod, opp, runner ^ 1);

            List<GameMove[]> altMoves = GameMove.GenerateMoves(moves, runner);

            Simulator sim = new Simulator();
            double maxU = Double.MinValue;
            int maxInd = 0;
            for (int i = 0; i < altMoves.Count; i++)
            {
                Situation situation = sim.SimulateHR(pod[0], pod[1], opp[0], opp[1], altMoves[i], runner);

                //Console.Error.WriteLine("thrust1: {0}, thrust2: {1}", altMoves[i][0].Thrust, altMoves[i][1].Thrust);
                //situation.Print();
                double utility = situation.UtilityHR();
                //Console.Error.WriteLine("Utility {0} = {1}", i, utility);
                if (utility > maxU)
                {
                    maxU = utility; maxInd = i;
                }
            }
            GameMove moveWithShield = GameMove.MoveWithShield(altMoves[maxInd][0]);
            if (runner == 0)
            {
                Situation sit = sim.Simulate(pod[0], pod[1], opp[0], opp[1], new GameMove[] {
                moveWithShield, altMoves[maxInd][1], altMoves[maxInd][2], altMoves[maxInd][3]});
                if (sit.UtilityHR() > maxU + 30 && !(pod[0].Lap == laps && pod[0].NextCP == 0))
                {
                    //moveWithShield.Print();
                    altMoves[maxInd][0] = moveWithShield;
                }
            }
            else
            {
                moveWithShield = GameMove.MoveWithShield(altMoves[maxInd][1]);
                Situation sit = sim.Simulate(pod[0], pod[1], opp[0], opp[1], new GameMove[] {
                altMoves[maxInd][0], moveWithShield, altMoves[maxInd][2], altMoves[maxInd][3]});
                if (sit.UtilityHR() > maxU + 40 && !(pod[1].Lap == laps && pod[1].NextCP == 0))
                {
                    //moveWithShield.Print();
                    altMoves[maxInd][1] = moveWithShield;
                }
            }

            

            return altMoves[maxInd];
        }
        public GameMove GetMove(Pod pod)
        {
            var strategy = new SimpleStrategy();
            //
            return strategy.GetMove(pod);
        }
    }
    class FastStrategy : IStrategy
    {
        public int Depth;
        public bool Opp = false;
        public FastStrategy(int depth = 4)
        {
            Depth = depth;
        }
        public GameMove[] GetMoves(Pod[] pod, Pod[] opp)
        {
            GameMove[] moves = new GameMove[4];
            moves = (new SimpleStrategy()).GetMoves(pod, opp);

            List<GameMove[]> altMoves = GameMove.GenerateMoves(moves);

            Simulator sim = new Simulator();
            double maxU = Double.MinValue;
            int maxInd = 0;
            for (int i = 0; i < altMoves.Count; i++)
            {
                Situation situation = sim.Simulate(pod[0], pod[1], opp[0], opp[1], altMoves[i], Opp);

                //Console.Error.WriteLine("thrust1: {0}, thrust2: {1}", altMoves[i][0].Thrust, altMoves[i][1].Thrust);
                //situation.Print();
                double utility = situation.Utility();
                //Console.Error.WriteLine("Utility {0} = {1}", i, utility);
                if (utility > maxU)
                {
                    maxU = utility; maxInd = i;
                }
            }
            GameMove moveWithShield = GameMove.MoveWithShield(altMoves[maxInd][0]);
            Situation sit = sim.Simulate(pod[0], pod[1], opp[0], opp[1], new GameMove[] {
                moveWithShield, altMoves[maxInd][1], altMoves[maxInd][2], altMoves[maxInd][3]});
            if (sit.Utility() > maxU + 30 && !(pod[0].Lap == laps && pod[0].NextCP == 0))
            {
                //moveWithShield.Print();
                altMoves[maxInd][0] = moveWithShield;
            }

            moveWithShield = GameMove.MoveWithShield(altMoves[maxInd][1]);
            sit = sim.Simulate(pod[0], pod[1], opp[0], opp[1], new GameMove[] {
                altMoves[maxInd][0], moveWithShield, altMoves[maxInd][2], altMoves[maxInd][3]});
            if (sit.Utility() > maxU + 40 && !(pod[1].Lap == laps && pod[1].NextCP == 0))
            {
                //moveWithShield.Print();
                altMoves[maxInd][1] = moveWithShield;
            }

            return altMoves[maxInd];
        }
        public GameMove GetMove(Pod pod)
        {
            var strategy = new SimpleStrategy();
            //
            return strategy.GetMove(pod);
        }
    }

    class SimpleStrategy : IStrategy
    {
        public GameMove DefendCP(Pod pod, Pod opp, Point CP)
        {
            double distanceToGoal = Math.Sqrt(Dist(CP, pod.p));
            //Console.Error.WriteLine("Distance to goal = {0}", distanceToGoal);
            double distanceFromGoalToSpeedLine = DistFromPointToLine(pod.p,
                new Point(pod.X + pod.VX, pod.Y + pod.VY), CP);
            //Console.Error.WriteLine("Distance from goal to speedline = {0}", distanceFromGoalToSpeedLine);

            bool shield = false;
            int thrust = 0;
            Point destPoint;
            destPoint = Destination(pod.p, CP, new Point(pod.X + pod.VX * 3, pod.Y + pod.VY * 3));

            bool lastCP = (pod.Lap == laps) && (pod.NextCP == 0);

            double speedAngle = SpeedAngle(pod.v);
            Point goalVector, nextGoalVector;
            goalVector.x = CP.x - pod.X;
            goalVector.y = CP.y - pod.Y;
            //nextGoalVector.x = cp[(pod.NextCP + 1) % C].x - pod.X;
            //nextGoalVector.y = cp[(pod.NextCP + 1) % C].y - pod.Y;
            nextGoalVector.x = opp.p.x - CP.x;
            nextGoalVector.y = opp.p.y - CP.y;
            double goalAngle = SpeedAngle(goalVector);
            double nextGoalAngle = SpeedAngle(nextGoalVector);
            //Console.Error.WriteLine("Speed Angle = {0}", speedAngle);
            //Console.Error.WriteLine("Goal Angle = {0}", goalAngle);
            double ourAngle = Math.Min(Math.Abs(speedAngle - goalAngle),
                Math.Min(Math.Abs(speedAngle - goalAngle - 360.0),
                Math.Abs(speedAngle - goalAngle + 360.0)));
            double anglediff = AngleDiff(goalAngle, nextGoalAngle);
            if (ourAngle > 90) destPoint = cp[pod.NextCP];

            //Console.Error.WriteLine(ourAngle);
            //double angle = Angle(cp[pod.NextCP], pod.p, pod.angle);
            double angle = Math.Min(Math.Abs(pod.Angle - goalAngle),
                Math.Min(Math.Abs(pod.Angle - goalAngle - 360.0),
                Math.Abs(pod.Angle - goalAngle + 360.0)));
            //Console.Error.WriteLine(angle);
            //Console.Error.WriteLine("Our angle = {0}", pod.Angle);

            //if (lastCP)
            //{
            //    //destPoint.Print();
            //    thrust = 220 - (int)(angle * angle) / 24;
            //    if (thrust < 80) thrust = 80;
            //    if (thrust > 200) thrust = 200;
            //    //return new GameMove(destPoint, thrust);
            //}
            //else 
            if (distanceFromGoalToSpeedLine < 480)
            {

                //Console.Error.WriteLine("Next Goal Angle = {0}", nextGoalAngle);
                double angle2 = AngleDiff(nextGoalAngle, pod.Angle);
                double timeToStart = Math.Abs(angle2) / 18.0;
                double V = pod.v.Module();
                //bool toBrake = 
                //double actualTime = distanceToGoal /
                //    Math.Sqrt(pod.VX * pod.VX + pod.VY * pod.VY);
                double actualTime = Double.MaxValue;
                if (0.15 * distanceToGoal < V)
                    actualTime = (-6.153) * Math.Log(1 - 0.15 * (distanceToGoal - 450) / V) + 0.2;
                //Console.Error.WriteLine("{0} {1}", timeToStart, actualTime);

                //double threshold = 0.85;
                //if (angle2 > 105) threshold = 0.5;
                //else if (angle2 < 30) threshold = 1.05;
                if (timeToStart > actualTime)
                {
                    // start braking
                    thrust = 0;
                    destPoint = opp.p;
                    pod.braking = true;
                    if (actualTime > 4 && distanceFromGoalToSpeedLine < 380) shield = true;
                }
                else
                {
                    thrust = 220 - (int)(angle * angle) / 13;
                    if (thrust < 65) thrust = 65;
                    if (thrust > 200) thrust = 200;
                    pod.braking = false;
                }
            }
            else {
                thrust = 225 - (int)(angle * angle) / 13;
                if (thrust < 65) thrust = 65;
                if (thrust > 200) thrust = 200;
                pod.braking = false;
            }
            return new GameMove(destPoint, thrust, shield);
        }
        public GameMove DefendCP(Pod pod, Pod opp, int CP)
        {
            return DefendCP(pod, opp, cp[CP]);
        }
        public GameMove GetMoveHit(Pod[] pods, Pod[] opps, int ind)
        {
            Pod[] allPods = new Pod[] { pods[0], pods[1], opps[0], opps[1] };
            return GetMoveHit(allPods, ind);
        }
        public GameMove GetMoveHit(Pod[] pods, int ind)
        {
            double u1 = pods[2].Utility();
            double u2 = pods[3].Utility();
            int toHunt = 2;
            //Console.Error.WriteLine("u1 = {0}, u2 = {1}", u1, u2);
            if (u2 > u1) toHunt = 3;
            double toNextCP = Math.Sqrt(Dist(pods[toHunt].p, cp[pods[toHunt].NextCP]));
            //double toNextCP1 = Math.Sqrt(Dist(pods[ind].p, cp[pods[toHunt].NextCP]));
            double toNextCP1 = DistFromPointToLine2(pods[toHunt].p, cp[pods[toHunt].NextCP], pods[ind].p);
            double distToOpp = Math.Sqrt(Dist(pods[ind].p, pods[toHunt].p));
            
            bool shield = false;
            Point dest = PointOnLineSegment(pods[toHunt].p, cp[pods[toHunt].NextCP], pods[ind].p);
            double angleDiff = AngleDiff(pods[ind].Angle, SpeedAngle(dest - pods[ind].p));
            int thrust = 80;
            string message = "";
            // can make better condition:
            if ((toNextCP1 < 0.7 * toNextCP && angleDiff < 140)|| distToOpp < 950)
            {
                message = "Hitting you " + toNextCP + " " + toNextCP1;

                if (distToOpp < 1500) shield = true;
                
                dest = new Point((dest*2 + cp[pods[toHunt].NextCP] + pods[toHunt].p) * 0.25);


                Point goalVector = dest - pods[ind].p;
                double goalAngle = SpeedAngle(goalVector);
                double angle = Math.Min(Math.Abs(pods[ind].Angle - goalAngle),
                    Math.Min(Math.Abs(pods[ind].Angle - goalAngle - 360.0),
                    Math.Abs(pods[ind].Angle - goalAngle + 360.0)));
                thrust = 215 - (int)(angle * angle) / 15;
                if (thrust < 80) thrust = 80;
                if (thrust > 200) thrust = 200;
                //if (toNextCP1 < 0.36 * toNextCP)
                //{
                //    dest = (dest + pods[toHunt].p) / 2.0;
                //    thrust -= 20;
                //}
                if (toNextCP1 < 0.22 * toNextCP)
                {
                    dest = pods[toHunt].p;
                    //thrust -= 50;
                }
            }
            else
            {
                double dist2 = Math.Sqrt(Dist(pods[ind].p, cp[pods[toHunt].NextCP]));
                
                //dest = pods[toHunt].p;
                Point pointToDefend = (cp[pods[toHunt].NextCP] * 4 + cp[(pods[toHunt].NextCP + 1) % C]*0 + pods[toHunt].p) / 5.0;
                
                //double alpha = 1 - toNextCP / Math.Sqrt(Dist(cp[(pods[toHunt].NextCP + C - 1) % C], cp[pods[toHunt].NextCP]));
                message = "Getting ready to hit you ";// + alpha;
                //if (dist2 * 0.17 < toNextCP)
                //{
                //    message = "; flying to other one";
                //    pointToDefend = (cp[pods[toHunt].NextCP] + cp[(pods[toHunt].NextCP + 1) % C] * 4 + pods[toHunt].p * 0) / 5.0;
                //}
                //double alpha2 = toNextCP1 / Math.Sqrt(Dist(cp[(pods[toHunt].NextCP + C - 1) % C], cp[pods[toHunt].NextCP]));
                //if (alpha > 0.7)
                //{
                //    alpha = 0.6;
                //    pointToDefend = (cp[pods[toHunt].NextCP] * (int)(1000*(1 - alpha)) + cp[(pods[toHunt].NextCP + 1) % C] * (int)(1000*alpha)) / 1000.0;
                //}
                if (pods[toHunt].NextCP == 0 && pods[toHunt].Lap == laps)
                {
                    pointToDefend = cp[0];
                }
                //GameMove res = DefendCP(pods[ind], pods[toHunt], pods[toHunt].NextCP);
                //GameMove res = DefendCP(pods[ind], pods[toHunt],
                //    (cp[pods[toHunt].NextCP] * 4 + cp[(pods[toHunt].NextCP + 1) % C]) / 5.0);
                //GameMove res = DefendCP(pods[ind], pods[toHunt],
                //    (pods[toHunt].p + cp[(pods[toHunt].NextCP + 1) % C]) / 2.0);
                GameMove res = DefendCP(pods[ind], pods[toHunt], pointToDefend);
                res.msg = message;
                return res;
                //dest = cp[(pods[toHunt].NextCP + 1) % C];

            }

         //   Console.Error.WriteLine("distToOpp = {0}", distToOpp);

            return new GameMove(dest, thrust, shield, message);
        }
        public GameMove[] GetMoves(Pod[] pod, Pod[] opp)
        {
            GameMove[] moves = new GameMove[4];
            moves[0] = GetMove(pod[0]);
            moves[1] = GetMove(pod[1]);
            return moves;
        }
        public GameMove GetMove(Pod pod)
        {
            double distanceToGoal = Math.Sqrt(Dist(cp[pod.NextCP], pod.p));
            //Console.Error.WriteLine("Distance to goal = {0}", distanceToGoal);
            double distanceFromGoalToSpeedLine = DistFromPointToLine(pod.p,
                new Point(pod.X + pod.VX, pod.Y + pod.VY), cp[pod.NextCP]);
            //Console.Error.WriteLine("Distance from goal to speedline = {0}", distanceFromGoalToSpeedLine);

            bool shield = false;
            int thrust = 0;
            Point destPoint;
            destPoint = Destination(pod.p, cp[pod.NextCP], new Point(pod.X + pod.VX * 3, pod.Y + pod.VY * 3));

            bool lastCP = (pod.Lap == laps) && (pod.NextCP == 0);

            double speedAngle = SpeedAngle(pod.v);
            Point goalVector, nextGoalVector;
            goalVector.x = cp[pod.NextCP].x - pod.X;
            goalVector.y = cp[pod.NextCP].y - pod.Y;
            //nextGoalVector.x = cp[(pod.NextCP + 1) % C].x - pod.X;
            //nextGoalVector.y = cp[(pod.NextCP + 1) % C].y - pod.Y;
            nextGoalVector.x = cp[(pod.NextCP + 1) % C].x - cp[pod.NextCP].x;
            nextGoalVector.y = cp[(pod.NextCP + 1) % C].y - cp[pod.NextCP].y;
            double goalAngle = SpeedAngle(goalVector);
            double nextGoalAngle = SpeedAngle(nextGoalVector);
            //Console.Error.WriteLine("Speed Angle = {0}", speedAngle);
            //Console.Error.WriteLine("Goal Angle = {0}", goalAngle);
            double ourAngle = Math.Min(Math.Abs(speedAngle - goalAngle),
                Math.Min(Math.Abs(speedAngle - goalAngle - 360.0),
                Math.Abs(speedAngle - goalAngle + 360.0)));
            double anglediff = AngleDiff(goalAngle, nextGoalAngle);
            if (ourAngle > 90) destPoint = cp[pod.NextCP];

            //Console.Error.WriteLine(ourAngle);
            //double angle = Angle(cp[pod.NextCP], pod.p, pod.angle);
            double angle = Math.Min(Math.Abs(pod.Angle - goalAngle),
                Math.Min(Math.Abs(pod.Angle - goalAngle - 360.0),
                Math.Abs(pod.Angle - goalAngle + 360.0)));
            //Console.Error.WriteLine(angle);
            //Console.Error.WriteLine("Our angle = {0}", pod.Angle);
            if (lastCP)
            {
                //destPoint.Print();
                thrust = 220 - (int)(angle * angle) / 24;
                if (thrust < 80) thrust = 80;
                if (thrust > 200) thrust = 200;
                //return new GameMove(destPoint, thrust);
            }
            else if (distanceFromGoalToSpeedLine < 480)
            {

                //Console.Error.WriteLine("Next Goal Angle = {0}", nextGoalAngle);
                double angle2 = AngleDiff(nextGoalAngle, pod.Angle);
                double timeToStart = Math.Abs(angle2) / 18.0;
                double V = pod.v.Module();
                //bool toBrake = 
                //double actualTime = distanceToGoal /
                //    Math.Sqrt(pod.VX * pod.VX + pod.VY * pod.VY);
                double actualTime = Double.MaxValue;
                if (0.15 * distanceToGoal < V)
                    actualTime = (-6.153) * Math.Log(1 - 0.15 * (distanceToGoal - 450) / V) + 0.2;
                //Console.Error.WriteLine("{0} {1}", timeToStart, actualTime);

                //double threshold = 0.85;
                //if (angle2 > 105) threshold = 0.5;
                //else if (angle2 < 30) threshold = 1.05;
                if (timeToStart > actualTime)
                {
                    // start braking
                    thrust = 0;
                    destPoint = cp[(pod.NextCP + 1) % C];
                    pod.braking = true;
                    if (actualTime > 4 && distanceFromGoalToSpeedLine < 380) shield = true;
                }
                else
                {
                    thrust = 220 - (int)(angle * angle) / 13;
                    if (thrust < 65) thrust = 65;
                    if (thrust > 200) thrust = 200;
                    pod.braking = false;
                }
            }
            else {
                thrust = 225 - (int)(angle * angle) / 13;
                if (thrust < 65) thrust = 65;
                if (thrust > 200) thrust = 200;
                pod.braking = false;
            }
            return new GameMove(destPoint, thrust, shield);
        }
    }

    #region geometry
    //static Point GetVector(int angle, int thrust)
    //{
    //    double dx, dy;
    //    dx = Math.Cos(angle * Math.PI / 180.0);
    //    dy = Math.Sqrt(1 - dx * dx);
    //    if (angle > 180) dy = -dy;
    //    //return new Point((int)Math.Round(thrust * dx), (int)Math.Round(thrust * dy));
    //    return new Point((int)(thrust * dx), (int)(thrust * dy));
    //}
    static PointD GetVector(int angle, int thrust)
    {
        double dx, dy;
        dx = Math.Cos(angle * Math.PI / 180.0);
        //dy = Math.Sqrt(1 - dx * dx);
        //if (angle > 180) dy = -dy;
        dy = Math.Sin(angle * Math.PI / 180.0);
        PointD res; res.x = thrust * dx; res.y = thrust * dy;
        return res;
    }
    static long Dist(Point p1, Point p2)
    {
        return (p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y);
    }
    //static double Angle(Point p1, Point p2, int angle)
    //{
    //    double res = Math.Atan((p2.y - p1.y) / (double)(p2.x - p1.x)) * 180.0 / Math.PI - angle;
    //    while (res < -90.0) res += 180.0;
    //    return res;
    //}

    static double AtanD(double numdenom)
    {
        return Math.Atan(numdenom) * 180.0 / Math.PI;
    }

    static double SpeedAngle(Point v)
    {
        if (v.x == 0) v.x = 1;
        if (v.x < 0 && v.y >= 0) { return 90.0 + AtanD((double)-v.x / v.y); }
        if (v.x > 0 && v.y >= 0) { return AtanD((double)v.y / v.x); }
        if (v.x > 0 && v.y < 0) { return 360.0 - AtanD((double)-v.y / v.x); }
        if (v.x < 0 && v.y < 0) { return 180.0 + AtanD((double)v.y / v.x); }
        return 0.0;
    }

    static double DistFromPointToLine(Point p1, Point p2, Point p0)
    {
        return Math.Abs((p2.y - p1.y) * p0.x - (p2.x - p1.x) * p0.y + p2.x * p1.y - p2.y * p1.x) /
            Math.Sqrt(Dist(p1, p2));
    }

    static double DistFromPointToLine2(Point p1, Point p2, Point p0)
    {
        Point p = PointOnLine(p1, p2, p0);
        if (p.x < p1.x && p.x < p2.x) return Double.MaxValue;
        if (p.x > p1.x && p.x > p2.x) return Double.MaxValue;
        if (p.y > p1.y && p.y > p2.y) return Double.MaxValue;
        if (p.y < p2.y && p.y < p1.y) return Double.MaxValue;

        double dist = Math.Abs((p2.y - p1.y) * p0.x - (p2.x - p1.x) * p0.y + p2.x * p1.y - p2.y * p1.x) /
            Math.Sqrt(Dist(p1, p2));

        return dist;
    }

    // straightens the ship by turning to optimal degree
    // should be used when the goal is to get to the destination point
    // in a fastest possible way
    static public Point Destination(Point p, Point cp, Point v)
    {
        Point projection = PointOnLine(p, cp, v);
        Point res = new Point(-v.x + 2 * projection.x, -v.y + 2 * projection.y);
        if (res.x == p.x && res.y == p.y) res = cp;
        return res;
    }

    static Point PointOnLine(Point pt1, Point pt2, Point pt)
    {
        //bool isValid = false;

        var r = new Point(0, 0);
        if (pt1.y == pt2.y && pt1.x == pt2.x) { return pt2; }

        double U = ((pt.y - pt1.y) * (pt2.y - pt1.y)) + ((pt.x - pt1.x) * (pt2.x - pt1.x));

        double Udenom = Math.Pow(pt2.y - pt1.y, 2) + Math.Pow(pt2.x - pt1.x, 2);

        U /= Udenom;

        r.y = (int)Math.Round(pt1.y + (U * (pt2.y - pt1.y)));
        r.x = (int)Math.Round(pt1.x + (U * (pt2.x - pt1.x)));

        return r;
    }

    // finds point on line segment (pt1, pt2), which is the closest to point pt
    // TODO: write this function
    static Point PointOnLineSegment(Point pt1, Point pt2, Point pt)
    {
        //bool isValid = false;

        var r = new Point(0, 0);
        if (pt1.y == pt2.y && pt1.x == pt2.x) { return pt2; }

        double U = ((pt.y - pt1.y) * (pt2.y - pt1.y)) + ((pt.x - pt1.x) * (pt2.x - pt1.x));

        double Udenom = Math.Pow(pt2.y - pt1.y, 2) + Math.Pow(pt2.x - pt1.x, 2);

        U /= Udenom;

        r.y = (int)Math.Round(pt1.y + (U * (pt2.y - pt1.y)));
        r.x = (int)Math.Round(pt1.x + (U * (pt2.x - pt1.x)));

        //double minx, maxx, miny, maxy;

        //minx = Math.Min(pt1.Y, pt2.Y);
        //maxx = Math.Max(pt1.Y, pt2.Y);

        //miny = Math.Min(pt1.X, pt2.X);
        //maxy = Math.Max(pt1.X, pt2.X);

        //isValid = (r.Y >= minx && r.Y <= maxx) && (r.X >= miny && r.X <= maxy);

        //return isValid ? r : new Point();
        return r;
    }

    static double AngleDiff(double angle1, double angle2)
    {
        return Math.Min(Math.Abs(angle1 - angle2),
            Math.Min(Math.Abs(angle1 - angle2 - 360.0),
            Math.Abs(angle1 - angle2 + 360.0)));
    }
    #endregion

    static string[] inputs;
    static Pod[] pod = new Pod[2];
    static Pod[] opp = new Pod[2];
    //static int[] lap = new int[2];
    static int laps;
    static int C;
    static List<Point> cp = new List<Point>(); // checkpoints
    static void InitialInput()
    {
        laps = int.Parse(Console.ReadLine());
        C = int.Parse(Console.ReadLine());
        for (int i = 0; i < C; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            Point point;
            point.x = int.Parse(inputs[0]);
            point.y = int.Parse(inputs[1]);
            cp.Add(point);
        }
        pod[0] = new Pod(); pod[1] = new Pod();
        opp[0] = new Pod(); opp[1] = new Pod();
    }

    static void Main(string[] args)
    {
        InitialInput();
        Simulator sim = new Simulator();
        int count = 0;
        int mainDepth = C - 1;
        while (true)
        {
            count++;
            for (int i = 0; i < 2; i++)
            {
                pod[i].Update(Console.ReadLine());
            }
            for (int i = 0; i < 2; i++)
            {
                opp[i].Update(Console.ReadLine());
            }
            var strategy = new SimpleStrategy();
            GameMove[] moves = new GameMove[4];
            if (count < 7)
            {
                moves[0] = strategy.GetMove(pod[0]);
                moves[1] = strategy.GetMove(pod[1]);
            }
            else if (count < 50)
            {
                var faststrategy = new FastStrategy();
                moves = faststrategy.GetMoves(pod, opp);
            }
            else
            {
                var hitandrun = new HitStrategy();
                moves = hitandrun.GetMoves(pod, opp);
            }
            if (count == 1)
            {
                moves[0].Thrust = 200;
                moves[1].Thrust = 200;
            }
            moves[0].Print();
            moves[1].Print();
        }
    }
}