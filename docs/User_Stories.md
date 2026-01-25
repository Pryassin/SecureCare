# User Stories & Acceptance Criteria: SecureCare Clinic Management System

## Epics Overview
1.  **Identity & Access Management (IAM)**
2.  **Patient Administration**
3.  **Clinic Scheduling**
4.  **Clinical Operations (EMR)**

---

## Epic 1: Identity & Access Management (IAM)

### US-1.1: Staff Account Creation
**As a** System Admin,
**I want to** create user accounts for Doctors and Receptionists and assign specific roles,
**So that** they can access the system with appropriate permissions.

**Acceptance Criteria:**
- [ ] Admin can input Username, Email, Password, and Role (Doctor/Receptionist).
- [ ] System hashes the password before saving (e.g., BCrypt).
- [ ] System prevents creating a user with a duplicate Email or Username.
- [ ] Only users with "Admin" role can access this feature.

### US-1.2: Secure Login
**As a** User (Admin, Doctor, Receptionist, Patient),
**I want to** log in using my credentials,
**So that** I can securely access my data.

**Acceptance Criteria:**
- [ ] User provides Username and Password.
- [ ] On success, system returns a JWT (JSON Web Token) containing claims (Id, Role).
- [ ] On failure, system returns a generic "Invalid Credentials" error.
- [ ] Passwords are never returned in the response.

---

## Epic 2: Patient Administration

### US-2.1: Register New Patient
**As a** Receptionist or Admin,
**I want to** register a new patient with their demographic details,
**So that** they can receive medical services.

**Acceptance Criteria:**
- [ ] Required fields: Name, National ID, DOB, Gender, Phone.
- [ ] System validates that National ID is unique.
- [ ] System validates that Email is unique (if provided).
- [ ] Upon saving, a unique Patient ID is generated.

### US-2.2: Patient Search
**As a** Staff Member (Receptionist, Doctor),
**I want to** search for existing patients by Name or National ID,
**So that** I can retrieve their records or book appointments.

**Acceptance Criteria:**
- [ ] Search returns partial matches for Name.
- [ ] Search returns exact matches for National ID.
- [ ] Results show minimal details (Name, DOB, ID) to confirm identity.

### US-2.3: Update Contact Information
**As a** Patient,
**I want to** update my phone number and address,
**So that** the clinic can contact me.

**Acceptance Criteria:**
- [ ] Authenticated Patient can only update their own record.
- [ ] Patient *cannot* update their National ID or Medical History.

---

## Epic 3: Clinic Scheduling

### US-3.1: View Available Slots
**As a** Receptionist or Patient,
**I want to** view available appointment slots for a specific doctor on a specific date,
**So that** I can choose a convenient time.

**Acceptance Criteria:**
- [ ] Input: Doctor ID, Date.
- [ ] Output: List of time slots that are not currently booked.
- [ ] Response time is under 300ms (NFR).

### US-3.2: Book Appointment
**As a** Receptionist,
**I want to** book an appointment on behalf of a patient,
**So that** I can schedule their visit.

**Acceptance Criteria:**
- [ ] System checks availability immediately before confirming (Double-booking prevention).
- [ ] Appointment status is set to "Pending" or "Confirmed".
- [ ] System links Patient ID and Doctor ID to the appointment.

### US-3.3: Doctor Schedule View
**As a** Doctor,
**I want to** view my own list of upcoming appointments,
**So that** I can prepare for my day.

**Acceptance Criteria:**
- [ ] Doctor sees only appointments assigned to them.
- [ ] List includes Patient Name and Time.
- [ ] Doctor cannot see appointments for other doctors.

---

## Epic 4: Clinical Operations (EMR)

### US-4.1: Record Prescription
**As a** Doctor,
**I want to** create a prescription for a completed appointment,
**So that** the medical advice is documented.

**Acceptance Criteria:**
- [ ] Can only create prescription if Appointment Status is "Completed".
- [ ] Can only create prescription for appointments assigned to the logged-in Doctor.
- [ ] Record includes Medication Details and Notes.
- [ ] Once saved, the record is **Immutable** (cannot be edited/deleted).

### US-4.2: View Personal Medical History
**As a** Patient,
**I want to** view my past appointments and prescriptions,
**So that** I can track my health history.

**Acceptance Criteria:**
- [ ] Patient sees a list of their own past appointments.
- [ ] Patient can click to view prescription details for those appointments.
- [ ] Patient receives a 403 Forbidden error if trying to view another patient's data.
