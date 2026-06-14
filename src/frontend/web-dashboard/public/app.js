const API_BASE = "http://localhost:5133";

// Inputs
const requestIdInput = document.getElementById("requestId");
const userNameInput = document.getElementById("userName");
const userRoleInput = document.getElementById("userRole");
const departmentInput = document.getElementById("department");
const requestTypeInput = document.getElementById("requestType");
const queryInput = document.getElementById("query");

// Outputs
const recommendationEl = document.getElementById("recommendation");
const confidenceEl = document.getElementById("confidence");
const policyReferenceEl = document.getElementById("policyReference");
const nextActionEl = document.getElementById("nextAction");
const policySummaryEl = document.getElementById("policySummary");
const citationEl = document.getElementById("citation");
const sourceSystemEl = document.getElementById("sourceSystem");
const timestampUtcEl = document.getElementById("timestampUtc");
const reasoningListEl = document.getElementById("reasoningList");
const explanationListEl = document.getElementById("explanationList");

const processBtn = document.getElementById("processBtn");

const sampleApproveBtn = document.getElementById("sampleApproveBtn");
const sampleEscalateBtn = document.getElementById("sampleEscalateBtn");
const sampleSecurityBtn = document.getElementById("sampleSecurityBtn");

const apiStatus = document.getElementById("apiStatus");

// Build request payload
function getPayload() {
  return {
    requestId: requestIdInput.value,
    userName: userNameInput.value,
    userRole: userRoleInput.value,
    department: departmentInput.value,
    requestType: requestTypeInput.value,
    query: queryInput.value
  };
}

// Render lists
function renderList(el, items) {
  el.innerHTML = "";
  if (!items || items.length === 0) {
    el.innerHTML = "<li>No data</li>";
    return;
  }
  items.forEach(i => {
    const li = document.createElement("li");
    li.textContent = i;
    el.appendChild(li);
  });
}

// Main call
async function processRequest() {
  const payload = getPayload();

  try {
    processBtn.disabled = true;

    const res = await fetch(`${API_BASE}/agent/process`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload)
    });

    if (!res.ok) {
      const text = await res.text();
      throw new Error(text);
    }

    const data = await res.json();

    //recommendationEl.textContent = data.recommendation;

    recommendationEl.innerHTML =
  `<span class="badge ${data.recommendation.toLowerCase()}">${data.recommendation}</span>`;

    confidenceEl.textContent = data.confidence;
    policyReferenceEl.textContent = data.policyReference;
    nextActionEl.textContent = data.nextAction;

    policySummaryEl.textContent = data.policySummary;
    citationEl.textContent = data.citation;
    sourceSystemEl.textContent = data.sourceSystem;
    timestampUtcEl.textContent = data.timestampUtc;

    renderList(reasoningListEl, data.reasoning);
    renderList(explanationListEl, data.explanation);

  } catch (err) {
    alert("Error: " + err.message);
  } finally {
    processBtn.disabled = false;
  }
}


sampleApproveBtn.addEventListener("click", () => {
  requestTypeInput.value = "Approval Review";
  queryInput.value =
    "Review this approval request and tell me whether it should be approved.";
});

sampleEscalateBtn.addEventListener("click", () => {
  requestTypeInput.value = "Approval Review";
  queryInput.value =
    "This request is incomplete and missing required data.";
});

sampleSecurityBtn.addEventListener("click", () => {
  requestTypeInput.value = "Security Review";
  queryInput.value =
    "This request affects security controls and protected systems.";
});



// Bind click
processBtn.addEventListener("click", processRequest);

// Health check
async function checkHealth() {
  try {
    const res = await fetch(`${API_BASE}/health`);
    apiStatus.textContent = res.ok ? "✅ Backend Connected" : "⚠️ API issue";
  } catch {
    apiStatus.textContent = "❌ Backend not reachable";
  }
}

checkHealth();