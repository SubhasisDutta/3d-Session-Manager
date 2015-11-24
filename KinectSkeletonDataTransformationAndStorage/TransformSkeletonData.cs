using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Cassandra;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    public class Cordinates
    {
        public double X;
        public double Y;
    }
    public class TransformSkeletonData
    {
        static void ExecuteTransformation()
        {
            String fileName = "C:\\temp\\Skeleton Lab Demo.txt";
            String transformfileName1 = "C:\\temp\\Skeleton Lab Demo_transform.txt";
            String transformfileName2 = "C:\\temp\\Skeleton Lab Demo_transform_angle.txt";

            Console.WriteLine("Parsing File : " + fileName);

            Dictionary<string, Cordinates> transformSkeleton = new Dictionary<string, Cordinates>();
            String metaData = null;
            String rawFile = null;

            using (StreamWriter sw = new StreamWriter(transformfileName1, true))
            {
                using (StreamReader sr = File.OpenText(fileName))
                {
                    rawFile = sr.ReadToEnd();
                    //TestReadingAndProcessingLinesFromFile_DoStuff(s);
                    List<String> frames = rawFile.Split(new string[] { "-------------EndFrame-------------" }, StringSplitOptions.None).ToList();
                    metaData = frames[0].Substring(0, frames[0].IndexOf(Environment.NewLine));
                    sw.WriteLine("Meta data : " + metaData);
                    Console.WriteLine("Meta data : " + metaData);
                    Dictionary<string, List<Cordinates>> medianDict = new Dictionary<string, List<Cordinates>>();

                    foreach (String frame in frames)
                    {
                        List<String> jointStrings = new List<string>();
                        using (StringReader reader = new StringReader(frame))
                        {
                            string line = string.Empty;
                            do
                            {
                                line = reader.ReadLine();
                                if (line != null)
                                {
                                    if (line.Contains("[JointPoints:"))
                                    {
                                        var t = line.Replace("[JointPoints:", "").Replace("]", "").Split(',');
                                        if (!medianDict.ContainsKey(t[0]))
                                        {
                                            Cordinates c = new Cordinates();
                                            c.X = Convert.ToDouble(t[1].Replace("X:", ""));
                                            c.Y = Convert.ToDouble(t[2].Replace("Y:", ""));
                                            List<Cordinates> p = new List<Cordinates>();
                                            p.Add(c);
                                            medianDict.Add(t[0], p);
                                        }
                                        else
                                        {
                                            Cordinates c = new Cordinates();
                                            c.X = Convert.ToDouble(t[1].Replace("X:", ""));
                                            c.Y = Convert.ToDouble(t[2].Replace("Y:", ""));
                                            medianDict[t[0]].Add(c);
                                        }
                                    }
                                }
                            } while (line != null);
                        }
                    }

                    foreach (var pair in medianDict)
                    {
                        sw.WriteLine(pair.Key);
                        Console.WriteLine(pair.Key);
                        List<Cordinates> sortedListX = pair.Value.OrderBy(o => o.X).ToList();
                        List<Cordinates> sortedListY = pair.Value.OrderBy(o => o.Y).ToList();
                        //Console.WriteLine(pair.Value.Count);
                        Cordinates c = new Cordinates();
                        c.X = sortedListX[pair.Value.Count / 2].X;
                        c.Y = sortedListY[pair.Value.Count / 2].Y;
                        transformSkeleton.Add(pair.Key, c);
                        sw.WriteLine("X= " + c.X);
                        Console.WriteLine("X= " + c.X);
                        sw.WriteLine("Y= " + c.Y);
                        Console.WriteLine("Y= " + c.Y);
                    }
                }
            }
            String transformString = null;
            StringBuilder ts = new StringBuilder();
            foreach (var pair in transformSkeleton)
            {
                ts.AppendFormat("[{0} : X={1},Y={2}]\n", pair.Key, pair.Value.X, pair.Value.Y);
            }
            transformString = ts.ToString();
            Console.Write(transformString);

            //Extraxt the angle
            using (StreamWriter sw = new StreamWriter(transformfileName2, true))
            {
                sw.Write(transformString);
                sw.WriteLine("--------------------------------------------------------------------");
                ts = new StringBuilder();
                double legAngle = Math.Abs(angleBetweenLines(transformSkeleton["SpineBase"], transformSkeleton["KneeLeft"], transformSkeleton["SpineBase"], transformSkeleton["KneeRight"]));
                double leftHandAngle = Math.Abs(angleBetweenLines(transformSkeleton["SpineBase"], transformSkeleton["SpineShoulder"], transformSkeleton["SpineShoulder"], transformSkeleton["ElbowLeft"]));
                double rightHandAngle = Math.Abs(angleBetweenLines(transformSkeleton["SpineBase"], transformSkeleton["SpineShoulder"], transformSkeleton["SpineShoulder"], transformSkeleton["ElbowRight"]));
                sw.WriteLine("Leg Angle : (SpineBase-KneeLeft,SpineBase-KneeRight) : " + legAngle);
                Console.WriteLine("Leg Angle : (SpineBase-KneeLeft,SpineBase-KneeRight) : " + legAngle);
                ts.AppendLine("Leg Angle : (SpineBase-KneeLeft,SpineBase-KneeRight) : " + legAngle);
                sw.WriteLine("Left Hand Angle : (SpineBase-SpineShoulder,SpineShoulder-ElbowLeft) : " + leftHandAngle);
                Console.WriteLine("Left Hand Angle : (SpineBase-SpineShoulder,SpineShoulder-ElbowLeft) : " + leftHandAngle);
                ts.AppendLine("Left Hand Angle : (SpineBase-SpineShoulder,SpineShoulder-ElbowLeft) : " + leftHandAngle);
                sw.WriteLine("Right Hand Angle : (SpineBase-SpineShoulder,SpineShoulder-ElbowRight) : " + rightHandAngle);
                Console.WriteLine("Right Hand Angle : (SpineBase-SpineShoulder,SpineShoulder-ElbowRight) : " + rightHandAngle);
                ts.AppendLine("Right Hand Angle : (SpineBase-SpineShoulder,SpineShoulder-ElbowRight) : " + rightHandAngle);
            }
            // Save it in database

            pushToDataStore(metaData, rawFile, transformString, ts.ToString());

            Console.ReadLine();
        }

        public static void pushToDataStore(String metaData, String rawFile, String jointdata2d, String angleData)
        {
            Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
            ISession session = cluster.Connect("kinectproject");

            var insertStatement = session.Prepare("insert into basickinectdatastore (id, metadata, rawdata, jointdata2d, angledata) values(?,?,?,?,?)");
            var batch = new BatchStatement();
            batch.Add(insertStatement.Bind(System.Guid.NewGuid(), metaData, rawFile, jointdata2d, angleData));
            // Insert Job
            session.Execute(batch);

        }

        public static double angleBetweenLines(Cordinates line1Point1, Cordinates line1Point2, Cordinates line2Point1, Cordinates line2Point2)
        {
            double slope1 = (line1Point2.Y - line1Point1.Y) / (line1Point2.X - line1Point1.X);
            double slope2 = (line2Point2.Y - line2Point1.Y) / (line2Point2.X - line2Point1.X);

            double temp = ((slope1 - slope2) / (1 + slope1 * slope2));
            double angle = Math.Atan(temp);
            return angle * (180 / Math.PI);
        }
    }


}
