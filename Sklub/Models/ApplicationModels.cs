using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sklub.Models
{
    public enum Measurement { Distance, Time, Points }

    public class Member
    {
        public int ID { get; set; }
        [Required]
        [DisplayName("Vorname")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Nachname")]
        public string LastName { get; set; }
        [DisplayName("Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        [DisplayName("Geburtsdatum")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Birthdate { get; set; }
        [DisplayName("Geschlecht")]
        public bool IsMale { get; set; }

        public string StreetInfo
        {
            get { return Street + " " + StreetNo; }
        }

        [DisplayName("Strasse")]
        public string Street { get; set; }
        [DisplayName("Nummer")]
        public string StreetNo { get; set; }
        [DisplayName("PLZ")]
        public int PostalCode { get; set; }
        [DisplayName("Ort")]
        public string City { get; set; }
        [DisplayName("Telefonnummer")]
        public string Phone { get; set; }
        [DisplayName("Mobiltelefon")]
        public string Mobile { get; set; }

        [DisplayName("STV Nummer")]
        public string ClubNo { get; set; }

        [EmailAddress]
        [DisplayName("E-Mail")]
        public string Email { get; set; }

        [ForeignKey("Account")]
        public string AccountID { get; set; }

        public virtual ApplicationUser Account { get; set; }
        public virtual ICollection<Section> Sections { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class Comment
    {
        public int ID { get; set; }

        [ForeignKey("Member")]
        public int MemberID { get; set; }
        public DateTime Created { get; set; }
        public string Message { get; set; }

        public virtual Member Member { get; set; }
    }

    public class Group
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Member> Members { get; set; }
    }

    public class Mutation
    {
        public int ID { get; set; }
        public DateTime Triggered { get; set; }
        public bool HasJoined { get; set; }

        [ForeignKey("Group")]
        public int GroupID { get; set; }

        [ForeignKey("Member")]
        public int MemberID { get; set; }

        public virtual Group Group { get; set; }
        public virtual Member Member { get; set; }
    }

    public class Section
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Color { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }

    public class Training
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DayOfWeek WeekDay { get; set; }

        [ForeignKey("Section")]
        public int SectionID { get; set; }

        public virtual Section Section { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }

    public class Lesson
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("Training")]
        public int TrainingID { get; set; }

        public virtual Training Training { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }

    public class Competition
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }

        public virtual ICollection<CompetitionCategory> Categories { get; set; }
    }

    public class CompetitionCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [ForeignKey("Competition")]
        public int CompetitionID { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }

    public class Result
    {
        public int ID { get; set; }
        public int Score { get; set; }

        [ForeignKey("Discipline")]
        public int DisciplineID { get; set; }

        [ForeignKey("MemberID")]
        public int MemberID { get; set; }
        
        public virtual Discipline Discipline { get; set; }
        public virtual Member Member { get; set; }
    }

    public class Discipline
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool HighestScoreWin { get; set; }
        public Measurement Measurement { get; set; }
    }
}