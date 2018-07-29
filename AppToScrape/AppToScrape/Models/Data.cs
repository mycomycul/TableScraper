using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppToScrape.Models
{
    public class PersonData
    {
        public int ID { get; set; }
        public string PersonName { get; set; }
        public int Nationality { get; set; }

        public PersonData(int id, int nationality, string Name)
        {
            ID = id;
            PersonName = Name;
            Nationality = nationality;

        }

    }
    public class Country
    {
        public int ID { get; set; }
        public string CountryName { get; set; }

        public Country(int id, string Name)
        {
            ID = id;
            CountryName = Name;
        }
    }
    public class SampleData
    {
        public List<Country> Countries;
        public List<PersonData> People;
        public int SelectedID;
        public int SelectedCountryID;
        public string SelectedName;

        public SampleData()
        {
            Countries = new List<Country>();
            People = new List<PersonData>();

            Countries.Add(new Country(1, "United Kingdom"));
            Countries.Add(new Country(2, "United States"));
            Countries.Add(new Country(3, "Portugal"));
            Countries.Add(new Country(4, "India"));

            People.Add(new PersonData(1, 1, "Bob"));
            People.Add(new PersonData(2, 2, "Mary"));
            People.Add(new PersonData(3, 3, "Kim"));
            People.Add(new PersonData(4, 4, "Michael"));
            People.Add(new PersonData(5, 3, "AJSON"));
            People.Add(new PersonData(6, 2, "Fred"));

        }
        public void SetSelected(int ID)
        {
            PersonData PD = People.Where(s => s.ID == ID).First();
            SelectedID = PD.ID;
            SelectedName = PD.PersonName;
            SelectedCountryID = PD.Nationality;
        }
    }
}