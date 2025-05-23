﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfTaskManager
{
    public class Task : Prototype
    {
        [Key] public int IdTask { get; set; }
        [ForeignKey("Projects")] private int idProject;
        [ForeignKey("Users")] private int idUser;
        private int timespent, isRunning;

        public int IdProject
        {
            get
            {
                return idProject;
            }
            set
            {
                if (idProject != value)
                {
                    idProject = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Timespent
        {
            get
            {
                return timespent;
            }
            set
            {
                if (timespent != value)
                {
                    timespent = value;
                    OnPropertyChanged();
                }
            }
        }

        public int IdUser
        {
            get
            {
                return idUser;
            }
            set
            {
                if (idUser != value)
                {
                    idUser = value;
                    OnPropertyChanged();
                }
            }
        }

        public int IsRunning
        {
            get
            {
                return isRunning;
            }
            set
            {
                isRunning = value;
                OnPropertyChanged();
            }
        }

        public Task() { }

        public Task(int idProject, string name, string description, DateTime startdate, DateTime deadline, int idUser)
        {
            IdProject = idProject;
            Name = name;
            Description = description;
            StartDate = startdate;
            Deadline = deadline;
            IdUser = idUser;
        }
    }
}
