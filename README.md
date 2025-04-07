
# 📅 Student Schedule Generator

This project is a **Student Schedule Generator** that helps university students build optimized class schedules based on their **individual preferences**. The main goal of this project is to improve the scheduling process by using **algorithmic logic**, not UI styling — the interface is kept basic on purpose.

> 💡 The focus of this project is **how the algorithm works**, not how it looks.

---

## 🚀 Project Goal

Scheduling classes manually can be time-consuming and frustrating for students. This tool simplifies that by:

- Automatically generating multiple schedule options
- Scoring each option based on the student’s preferences
- Displaying the **top 3 best schedules** to choose from

---

## 🧠 How It Works (Algorithm Logic)

The scheduling process follows these steps:

1. **Retrieve Data** (from the database) — includes all available course sections provided by the university and the student’s major plan
2. **Collect Preferences** (from student input)
3. **Create Population** (possible schedule combinations)
4. **Calculate Fitness Score** (based on time, preferred instructor , etc.)
5. **Select Top 3** highest scoring schedules
6. **Display Results** to the student

### 🔄 Flowchart

![MyScheduler flow chart](https://github.com/user-attachments/assets/2807ab8c-2d55-43e0-a23d-7c341799178e)

---

## 🧱 System Architecture

The system is built using **Object-Oriented Principles**, with a focus on **interfaces**, **modular classes**, and **fitness-based evaluation**.

### 🧩 UML Class Diagram

![MyScheduler](https://github.com/user-attachments/assets/5aa17182-490b-4c06-91ab-7a1ab3fa535f) 

---

## 🛠 Technologies Used

- ASP.Net core
- Visual Paradigm for UMLs
- Custom algorithm for fitness evaluation

---

## 🧪 Fitness Criteria

Each schedule is scored based on:

- **Instructor Score** (student preferred instructor)
- **Time/Days Score** (preferences for specific days or times)

These components are aggregated to calculate the **overall fitness score** for each schedule.

---

## 📌 Notes

- The interface is **basic with no advanced styling**.
- Focus is on **backend logic and algorithm design**.
- All generated schedules are conflict-free (no overlapping class times).

---

## 📷 Screenshots
🔢 User Steps
The following UI steps collect student preferences. Each selection is used as input for the scheduling algorithm.
![Paper (1)](https://github.com/user-attachments/assets/0a09c850-8d85-40f6-8d4f-c177599df5d7)
🗂️ Results
Below are sample results showing generated schedules that match student preferences without any conflicts.
![Screenshot 2024-09-23 103644](https://github.com/user-attachments/assets/0a74ddd6-0ce5-4448-abfb-e0d6ebe05e37)
![Screenshot 2024-09-23 103702](https://github.com/user-attachments/assets/e4e60a1f-0871-43f0-9d92-cd1ed5e80ea4)


---

## 🙋‍♂️ Author

Built by a passionate software engineer aiming to ease university life using smart automation. 😊
