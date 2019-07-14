﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Person : IModel<Person>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string WorkArea { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }

        public virtual bool EqualByMembers(Person person)
        {
            return person != null &&
                   Name == person.Name &&
                   WorkArea == person.WorkArea &&
                   Age == person.Age &&
                   Gender == person.Gender;
        }

        public virtual int GetHashCodeWithMembers()
        {
            unchecked
            {
                var hashCode = 397;
                hashCode = (hashCode * 397) ^ (Name?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (WorkArea?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Age.GetHashCode();
                hashCode = (hashCode * 397) ^ Gender.GetHashCode();
                return hashCode;
            }
        }
    }
}
