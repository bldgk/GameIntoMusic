using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiM.Classes.Data_Classes
{
    public class Note
    {
        public Guid Id { get; set; }

       // public Guid TrackId { get { return Track.Id; } }

       // public virtual Track Track { get; set; }

        public string Name
        {
            get { return Degree.ToString() + Type.ToString() + Octave.ToString() + Alteration.ToString(); }
        }

        public DegreeNote Degree
        { get; set; }
        public TypeNote Type
        {
            get;
            set;
        }


        public OctaveNote Octave
        {
            get;
            set;
        }
        public AlterationNote Alteration { get; set; }


        public Note() { }
        public Note(DegreeNote degree, TypeNote type, OctaveNote octave, AlterationNote alteration)
        {
            Id = Guid.NewGuid();
            this.Degree = degree;
            this.Type = type;
            this.Octave = octave;
            this.Alteration = alteration;
        }
    }

    public enum TypeNote
    {
        Quarter, Whole, Sixteenth, Eighth, Half
    }
    public enum OctaveNote
    {
        First, Second, Third, Small
    }
    public enum DegreeNote
    {
        C, D, E, F, G, A, H
    }
    public enum AlterationNote
    {
        None, Flatted, Sharped
    }
}