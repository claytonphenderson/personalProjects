using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace AttendanceProof
{
    public class Program
    {
        //Static Values
        public static string connectionString = "Server=RAIKES-DS0324;Database=Westside;Trusted_Connection=True;MultipleActiveResultSets=true";
        
        //Given Values
        public static int TeacherID = 1;
        public static int CourseID = 101;
        public static int MeetingID = 1;

        public static int[] StudentID = { 101, 102, 103, 104, 105, 106, 107, 108, 109, 110 };   

        public static int RetrievedPSCourse;
        public static int RetrievedPSSection;
        //
        public static void Main(string[] args)
        {
            StepOne();
            Console.WriteLine("--------------Step One Completed");
            Console.WriteLine("");
            StepTwo();
            Console.WriteLine("--------------Step Two Completed");
            Console.WriteLine("");
            StepThree();
            Console.WriteLine("--------------Step Three Completed");
            Console.WriteLine("");
            Console.ReadKey();
        }
        /// <summary>
        /// Using the given Vars, retrieve the PowerSchool
        /// Course and Section from the PowerSchool Relation Table
        /// </summary>
        public static void StepOne()
        {
            Console.WriteLine(":::::::Getting PS Course and Section Using:::::::");
            Console.WriteLine("       TeacherID: " + TeacherID);
            Console.WriteLine("       CourseID: " + CourseID);
            Console.WriteLine("       MeetingID: " + MeetingID);

            string queryString = "SELECT * FROM PowerSchoolRelation WHERE TeacherID = " + TeacherID + " and CourseID = " + CourseID + " and MeetingID=" + MeetingID;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        RetrievedPSCourse = (int)reader[3];
                        RetrievedPSSection = (int)reader[4];
                    }
                    conn.Close();
                }catch(Exception e)
                {
                    conn.Close();
                    throw new Exception(e.Message);
                }
            }
        }
        /// <summary>
        /// Use the PowerSchool CourseID and SectionID to create
        /// a new entry to the PowerSchool Attendance Table
        /// </summary>
        public static void StepTwo()
        {
            if(RetrievedPSSection==0 && RetrievedPSCourse==0)
            {
                throw new Exception("No data found for give teacher, meeting, and course info");
            }
            Console.WriteLine(":::::::Creating New Attendance Entry Using:::::::");
            Console.WriteLine("       Retrieved PS Course: " + RetrievedPSCourse);
            Console.WriteLine("       Retrieved PS Section: " + RetrievedPSSection);
            Console.WriteLine("");
            Console.WriteLine("Press any key to add new Attendance Data to Table");
            Console.ReadKey();
            foreach (int i in StudentID)
            {
                string queryString = "INSERT INTO PowerSchoolAttendance (StudentID, Date, PowerSchoolCourseID, PowerSchoolSectionID, Code) VALUES (" + i + ",'" + getCurrentDate() + "', " + RetrievedPSCourse + "," + RetrievedPSSection + ", 0)";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(queryString, conn);
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        Console.WriteLine("");
                        Console.WriteLine("Record added to PowerSchool Attendance");
                    }
                    catch (Exception e)
                    {
                        conn.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// Export all data existing in Attendance Table
        /// </summary>
        public static void StepThree()
        {
            Console.WriteLine("::::::::All data in Attendance Table:::::::");

            string queryString = "SELECT * FROM PowerSchoolAttendance";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, conn);
                try
                {
                    int count = 1;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var a = reader[0];
                        var b = reader[1];
                        var c = reader[2];
                        var d = reader[3];
                        var e = reader[4];

                        Console.WriteLine(count);
                        Console.WriteLine("Student ID: "+a);
                        Console.WriteLine("Date: "+b);
                        Console.WriteLine("PowerSchool Course ID: "+c);
                        Console.WriteLine("PowerSchool Section ID: "+ d);
                        Console.WriteLine("Code: "+e);
                        Console.WriteLine("-----------------------------------------------");
                        count++;
                    }
                    conn.Close();
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw new Exception(e.Message);
                }
            }
        }
        public static DateTime getCurrentDate()
        {
            DateTime dt = DateTime.Now;
            return dt;
        }
    }
}
