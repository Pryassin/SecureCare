# API Specification: SecureCare CMS

## Overview
Base URL: `/api/v1`
Format: JSON
Error Format: standard `ProblemDetails` (RFC 7807)

## 1. Authentication

### POST /auth/login
Authenticate a user and retrieve a JWT token.

**Request Body:**
```json
{
  "username": "dr.smith",
  "password": "secure_password"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI...",
  "expiresAt": "2026-01-25T14:00:00Z"
}
```

**Response (401 Unauthorized):**
Invalid credentials.

---

## 2. Patients

### POST /patients
Register a new patient profile.
*Authorization: Receptionist, Admin*

**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "nationalId": "123456789",
  "dateOfBirth": "1985-05-20",
  "gender": "Male",
  "email": "john@example.com",
  "phone": "+1234567890",
  "password": "initial_password_123"
}
```

**Response (201 Created):**
```json
{
  "id": "guid-uuid-guid"
}
```

### GET /patients/search?q={query}
Search for patients by name or national ID.
*Authorization: Receptionist, Doctor, Admin*

**Response (200 OK):**
```json
[
  {
    "id": "guid-uuid",
    "fullName": "John Doe",
    "nationalId": "1234***89",
    "dateOfBirth": "1985-05-20"
  }
]
```

---

## 3. Appointments

### GET /appointments/slots
Get available time slots for a doctor.
*Authorization: Authenticated Users*

**Query Params:**
*   `doctorId`: (guid)
*   `date`: (yyyy-mm-dd)

**Response (200 OK):**
```json
[
  "09:00",
  "09:30",
  "14:00"
]
```

### POST /appointments
Book a new appointment.
*Authorization: Receptionist, Patient*

**Request Body:**
```json
{
  "doctorId": "guid-uuid",
  "patientId": "guid-uuid",
  "dateTime": "2026-02-01T09:00:00Z"
}
```

**Response (200 OK):**
```json
{
  "appointmentId": "guid-uuid",
  "status": "Pending"
}
```

---

## 4. Clinical (EMR)

### POST /medical/prescriptions
Create a medical record for a completed appointment.
*Authorization: Doctor Only*

**Request Body:**
```json
{
  "appointmentId": "guid-uuid",
  "medication": "Amoxicillin 500mg - 3x daily",
  "notes": "Patient reported mild fever."
}
```

**Response (201 Created):**
```json
{
  "id": "guid-uuid"
}
```

### GET /medical/prescriptions/{appointmentId}
View a prescription.
*Authorization: Doctor (Owner), Patient (Owner)*

**Response (200 OK):**
```json
{
  "id": "guid-uuid",
  "medication": "Amoxicillin 500mg...",
  "notes": "Patient reported...",
  "createdAt": "2026-02-01T09:30:00Z"
}
```
