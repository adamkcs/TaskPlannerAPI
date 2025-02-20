# Task Manager API

Description

## Db relations
┌──────────────────────────┐       ┌───────────────────┐       ┌──────────────────┐
│         Board            │ 1───▶ │     TaskList      │ 1───▶ │    TaskItem      │
│ ───────────────────────  │       │ ────────────────  │       │ ─────────────── │
│ Id        (PK)           │       │ Id      (PK)      │       │ Id      (PK)    │
│ Name                     │       │ Name             │       │ Title           │
│ Description (optional)    │       │ BoardId (FK)     │       │ Description     │
│ TaskLists (Navigation)    │       │ Tasks (Navigation) │    │ DueDate         │
└──────────────────────────┘       └───────────────────┘       │ Priority        │
                                                                │ Status         │
                                                                │ TaskListId (FK) │
                                                                │ AssignedUserId (FK) │
                                                                │ Comments (Navigation) │
                                                                │ Labels (Navigation) │
                                                                └──────────────────┘

┌───────────────────────────┐     ┌───────────────────────┐
│        Comment            │     │       Label           │
│ ────────────────────────  │     │ ────────────────────  │
│ Id        (PK)            │     │ Id      (PK)          │
│ Content                   │     │ Name                 │
│ CreatedAt                 │     │ Tasks (Navigation)   │
│ UserId (FK)               │     └──────────────────────┘
│ TaskItemId (FK)           │
└───────────────────────────┘

┌───────────────────────────┐
│        User (Identity)    │
│ ────────────────────────  │
│ Id        (PK)            │
│ UserName                 │
│ Email                    │
│ Tasks (Navigation)       │
└───────────────────────────┘
