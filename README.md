# TaskPlanner Frontend

This is the **React/Next.js** frontend for the **TaskPlanner API**, built with TypeScript. It provides user authentication, a protected dashboard, and integration with a **.NET backend**.

## 🚀 Features

✅ **User Authentication** (JWT-based)  
✅ **Login & Registration**  
✅ **Protected Routes** (AuthGuard & Middleware)  
✅ **Global State Management** with React Context  
✅ **API Integration** with Axios  
✅ **Next.js Pages & Routing**  

---

## 🛠️ Tech Stack

- **Frontend:** React, Next.js, TypeScript  
- **State Management:** React Context API  
- **Authentication:** JWT (stored in `localStorage`)  
- **HTTP Client:** Axios  
- **Backend:** `.NET TaskPlanner API`  

---

## 📂 Project Structure
/task-planner-frontend 
│── /components # Reusable UI components 
│── /context # AuthContext for authentication 
│── /pages 
│ ├── index.tsx # Landing page 
│ ├── login.tsx # Login page 
│ ├── register.tsx # Registration page 
│ ├── dashboard.tsx # Protected dashboard 
│── /services # API calls (login, register, session) 
│── /utils # Helper functions 
│── middleware.ts # Next.js middleware for route protection 
│── _app.tsx # Global app wrapper 
│── README.md # Documentation

## 🔧 Setup & Installation
git clone https://github.com/your-repo/task-planner-frontend.git
cd task-planner-frontend
npm install
npm run dev
http://localhost:3001
