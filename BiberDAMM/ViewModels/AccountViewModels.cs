﻿using System.ComponentModel.DataAnnotations;
using BiberDAMM.Models;

namespace BiberDAMM.ViewModels
{
    //[KrabsJ]
    public class LoginViewModel
    {
        //section changed: login should be based on username instead of Email [KrabsJ]
        /*
        [Required]
        [Display(Name = "E-Mail")]
        [EmailAddress]
        public string Email { get; set; }
        */

        [Required]
        [Display(Name = "Benutzername")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Kennwort")]
        public string Password { get; set; }

        //section deleted: for security reasons there should be no rememberme function [KrabsJ]
        /*
        [Display(Name = "Speichern?")]
        public bool RememberMe { get; set; }
        */
    }

    public class RegisterViewModel
    {
        // section added: for registration of a new user the Surname, Lastname, UserType and ActiveFlag are needed; phoneNumber and title are optional [KrabsJ]
        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Vorname")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Nachname")]
        public string Lastname { get; set; }

        [Required]
        [Display(Name = "Benutzertyp")]
        public UserType UserType { get; set; }

        [Display(Name = "Aktiviert")]
        public bool Active { get; set; }

        [Display(Name = "Telefonnummer")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "\"{0}\" muss mindestens {2} Zeichen lang sein.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Kennwort")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Kennwort bestätigen")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage =
            "Das Kennwort entspricht nicht dem Bestätigungskennwort.")]
        public string ConfirmPassword { get; set; }
    }

    // ViewModel for editing applicationUsers [KrabsJ]
    public class EditViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Vorname")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Nachname")]
        public string Lastname { get; set; }

        [Required]
        [Display(Name = "Benutzertyp")]
        public UserType UserType { get; set; }

        [Display(Name = "Aktiviert")]
        public bool Active { get; set; }

        [Display(Name = "Telefonnummer")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }
    }

    // ViewModel for creating a new password for an user by administrator [KrabsJ]
    public class NewInitialPasswordViewModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "\"{0}\" muss mindestens {2} Zeichen lang sein.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Kennwort")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Kennwort bestätigen")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage =
            "Das Kennwort stimmt nicht mit dem Bestätigungskennwort überein.")]
        public string ConfirmPassword { get; set; }
    }
}