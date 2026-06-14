🖥️ CARE‑NEXUS AI Web Dashboard
Frontend Interface for Enterprise Workflow Intelligence

🚀 Overview
The CARE‑NEXUS AI Web Dashboard is a responsive, browser-based interface that enables users to interact with the CARE‑NEXUS AI backend.
It provides a Copilot-style experience where users can:

Submit workflow requests
Simulate real-world scenarios
View AI-generated decisions
Understand reasoning and policy citations


🎯 Key Features
✅ Request Input Panel

Capture workflow request details:

Request ID
User Name
Role & Department
Request Type
Query




✅ Scenario Simulation
Quick testing using predefined buttons:

🟢 Approval Scenario
🟡 Escalation Scenario
🔴 Security Scenario

Allows one-click input population for demo purposes.

✅ Decision Output Panel
Displays AI-generated results:

✅ Recommendation (Approve / Escalate / Security)
✅ Confidence level
✅ Policy reference
✅ Next action
✅ Timestamp


🧠 Explainability (Foundry IQ Alignment)
The dashboard visualises:

📄 Policy Summary
🔗 Citation (SharePoint document source)
🏢 Source System
🧾 Reasoning (step-by-step logic)
📊 Explanation (detailed decision breakdown)


✅ API Status Monitoring

Displays backend health status
Confirms live API connectivity


⚙️ Technology Stack

HTML5 – Structure
CSS3 – Styling and layout
JavaScript (Vanilla) – API interaction and UI logic


🔗 Backend Integration
The dashboard connects to the backend API:
POST /agent/process
GET  /health

Example API configuration
JavaScriptconst API_BASE = "http://localhost:5133";Show more lines

🧪 How to Run
1. Ensure Backend is Running
Shellcd backend/apidotnet runShow more lines

2. Start Frontend
Open the dashboard:
src/frontend/web-dashboard/public/index.html

👉 Recommended:

Use Live Server (VS Code)
Or run a static server:

Shellnpx serve .Show more lines

3. Verify Connection
You should see:
✅ Backend Connected


🧪 Demo Workflow
Step 1:
Click a scenario button (Approval / Escalate / Security)
Step 2:
Click Process Request
Step 3:
View results:

Recommendation
Policy
Explanation
Citation


📂 Folder Structure
web-dashboard/
 ├── README.md
 ├── package.json
 └── public/
     ├── index.html
     ├── styles.css
     └── app.js


🎨 UI Design Highlights

✅ Clean enterprise-style dashboard
✅ Grid-based layout for readability
✅ Styled output panels
✅ Colored recommendation badges
✅ Responsive design


⚠️ Notes

Requires backend API to be running
Uses HTTP (localhost) for demo
Best viewed in Chrome / Edge


🔮 Future Enhancements

UI animations and transitions
Loading spinner for API calls
Dark/light theme toggle
Integration with Microsoft Teams / Copilot
Mobile-optimized design


🏁 Summary
The CARE‑NEXUS AI Web Dashboard provides:
✅ A user-friendly interface
✅ Real-time AI interaction
✅ Explainable decision outputs
✅ Seamless integration with enterprise backend

👨‍💻 Author
Abdul Ali Qureshi
HICT Specialist – Healthcare IT & AI Solutions
