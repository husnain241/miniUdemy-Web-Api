# 🎓 Mini Udemy Platform - ASP.NET Core (.NET 8)

This is a Mini Udemy-style online learning platform built with **ASP.NET Core (.NET 8)** and **Entity Framework Core**. It supports **user authentication, course & lesson management, teacher approval workflow, and student enrollment** — all powered by a clean API-based architecture.

---

## 🚀 Main Features

### 1. User Authentication & Role Management
- Users can **register** and **log in**.
- Supports **JWT authentication** for secure API access.
- Three roles:  
  - `Student`  
  - `Teacher`  
  - `Admin`

### 2. Course Management
- Teachers can:
  - Create, update, and delete **their own courses**.
- All users (including anonymous) can:
  - View all available courses.

### 3. Lesson Management
- Teachers can:
  - Add, update, and delete **lessons** inside their courses.
- All users can:
  - View lessons for any course.

### 4. Enrollment System
- Students can:
  - Enroll in courses.
  - View their enrolled courses.
  - Cancel their own enrollments.
- Teachers can:
  - View enrolled students in their courses.

### 5. Teacher Application & Admin Approval
- Users can apply to become teachers.
- Admins can:
  - Approve or reject teacher applications via API.

---

## 🧠 Architecture & Technology Stack

- **ASP.NET Core (.NET 8) Web API**
- **Entity Framework Core** (with Code First & AppDbContext)
- **JWT Authentication**
- **ASP.NET Core Identity** for role & user management
- **Repository Pattern** for data access abstraction
- **DTOs** (Data Transfer Objects) for clean API contracts
- **Swagger** (Swashbuckle) for API testing/documentation

---

## 🔄 How It Works

| Action | Flow |
|--------|------|
| Register/Login | User signs up and receives a JWT token for future authenticated requests |
| Apply as Teacher | User submits request, which is reviewed by an Admin |
| Create Course | Approved teachers create and manage courses |
| Add Lessons | Teachers manage lessons under their own courses |
| Enroll | Students enroll in courses and view lessons |
| Admin Actions | Admins manage teacher approval workflow |

---

## 🛠️ How to Run Locally

### Prerequisites
- Visual Studio 2022
- .NET 8 SDK
- SQL Server
- Postman or Swagger UI

### Steps

1. **Clone the repository:**
   ```bash
   git clone https://github.com/your-username/mini-udemy-api.git
