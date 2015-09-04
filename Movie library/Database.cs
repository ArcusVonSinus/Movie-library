using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie_library
{
    class Database
    {
        public int selectedIndex;        
        public List<Film> films;
        public int pocetFilmu
        {
            get { return films.Count; }
        }
        public Film selectedFilm
        {
            get
            {
                return films[selectedIndex];
            }
        }
        public Database()
        {
            films = new List<Film>();
            selectedIndex = 0;
        }
        public string[] posters()
        {
            string[] temp = new string[films.Count];
            for (int i = 0; i < films.Count; i++)
            {
                temp[i] = films[i].posterPath;
            }
            return temp;
        }

    }
    class Film
    {
        public string name;
        public string path;
        public string posterPath;
        public Film(string name, string path, string posterPath)
        {
            this.name = name;
            this.path = path;
            this.posterPath = posterPath;
        }

    }
}
