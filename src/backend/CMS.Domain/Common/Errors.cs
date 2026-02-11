namespace CMS.Domain.Common;
public static class Errors
{
    public static class User
    {
        public static Error NotFound=>new("User.NotFound","User not found");
         public static Error InvalidEmail => new("User.InvalidEmail", "Invalid email format");
        public static Error AlreadyExists => new("User.AlreadyExists", "User already exists");
        public static Error EmailRequired=>new("User.EmailRequired","Email is required");
        public static Error PasswordRequired=>new ("User.PasswordRequired","Password is Required");
        public static Error InvalidRole=>new ("User.InvalidRole","Role is not valid");
    }
    public static class Doctor
    {
        public static Error NotFound => new("Doctor.NotFound", "Doctor not found");
        public static Error AlreadyExists => new("Doctor.AlreadyExists", "Doctor profile already exists for this user");
        public static Error UserIdRequired => new(
            "Doctor.UserIdRequired", "A valid User ID is required to create a doctor profile.");
        public static Error LicenseRequired => new(
            "Doctor.LicenseRequired", "The medical license number is required.");
        public static Error SpecializationRequired => new(
            "Doctor.SpecializationRequired", "The specialization field cannot be empty.");
    }
    
    public static class Appointment
    {
        public static Error PatientIdRequired => new("Appointment.PatientIdRequired", "A valid Patient ID is required.");
        public static Error DoctorIdRequired => new("Appointment.DoctorIdRequired", "A valid Doctor ID is required.");
        public static Error PastDate => new("Appointment.PastDate", "Appointment date cannot be in the past.");
        public static Error Conflict => new("Appointment.Conflict", "The selected time slot is already booked.");
        public static Error NotFound=>new ("Appointment.NotFound", "Appointment not found.");
    }

    public static class Patient
    {
        public static Error NotFound => new("Patient.NotFound", "Patient not found");
        public static Error AlreadyExists => new("Patient.AlreadyExists", "Patient profile already exists for this user");
        public static Error DuplicateNationalId => new("Patient.DuplicateNationalId", "A patient with this national ID already exists");
        public static Error UserIdRequired => new("Patient.UserIdRequired", "User ID is required.");
        public static Error NationalIdRequired => new("Patient.NationalIdRequired", "National ID is required.");
        public static Error PhoneRequired => new("Patient.PhoneRequired", "Phone number is required.");
    }

    public static class Prescription
    {
        public static Error NotFound => new("Prescription.NotFound", "Prescription not found");
        public static Error InvalidStatus => new("Prescription.InvalidStatus", "Prescription can only be created for completed appointments");
        public static Error AppointmentIdRequired => new("Prescription.AppointmentIdRequired", "Appointment ID is required.");
        public static Error MedicationDetailsRequired => new("Prescription.MedicationDetailsRequired", "Medication details are required.");
    }

    public static class Auth
    {
        public static Error InvalidCredentials => new("Auth.InvalidCredentials", "Invalid email or password");
    }
}