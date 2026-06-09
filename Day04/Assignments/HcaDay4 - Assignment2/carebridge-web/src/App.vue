<script setup>
import { ref, onMounted } from 'vue'

const maxTotal = ref(0)
const departmentLoads = ref([])
const fromDate = ref('')
const sortBy = ref('Total')

async function loadDepartmentData() {

  let url =
    `https://localhost:7082/api/analytics/department-load?sortBy=${sortBy.value}`

  if (fromDate.value) {

    url +=
      `&fromDate=${fromDate.value}`
  }

  const response =
    await fetch(url)

  departmentLoads.value =
    await response.json()

  maxTotal.value =
    Math.max(
      ...departmentLoads.value.map(d => d.total)
    )
}

onMounted(async () => {
  await loadDepartmentData()
})
</script>

<template>

  <div class="header">

    <h1>CareBridge Department Dashboard</h1>

    <div class="filters">

      <div class="filter-item">
        <label>From Date:</label>
        <input
          type="date"
          v-model="fromDate"
          @change="loadDepartmentData"
        />
      </div>

      <div class="filter-item">
        <label>Sort By:</label>
        <select
          v-model="sortBy"
          @change="loadDepartmentData"
        >
          <option value="Total">Total</option>
          <option value="Department">Department</option>
        </select>
      </div>

    </div>

  </div>

  <table border="1">

    <tr>
      <th>Department</th>
      <th>Inpatient</th>
      <th>Outpatient</th>
      <th>ED</th>
      <th>Total</th>
    </tr>

    <tr
      v-for="d in departmentLoads"
      :key="d.departmentName"
      :class="{ busiest: d.total === maxTotal }"
    >
      <td>{{ d.departmentName }}</td>
      <td>{{ d.inpatient }}</td>
      <td>{{ d.outpatient }}</td>
      <td>{{ d.ed }}</td>
      <td>{{ d.total }}</td>
    </tr>

    <tr class="grand-total">
      <td>Grand Total</td>
      <td>{{ departmentLoads.reduce((sum, d) => sum + d.inpatient, 0) }}</td>
      <td>{{ departmentLoads.reduce((sum, d) => sum + d.outpatient, 0) }}</td>
      <td>{{ departmentLoads.reduce((sum, d) => sum + d.ed, 0) }}</td>
      <td>{{ departmentLoads.reduce((sum, d) => sum + d.total, 0) }}</td>
    </tr>

  </table>

</template>

<style scoped>
.header {
  margin-top: 20px;
  margin-bottom: 20px;
}

.header h1 {
  text-align: center;
  margin-bottom: 40px;
}

.filters {
  display: flex;
  justify-content: center;
  gap: 20px;
  flex-wrap: wrap;
  margin-top: 20px;
}

.filter-item {
  display: flex;
  align-items: center;
  gap: 8px;
}

.busiest {
  background-color: rgb(255, 150, 129);
  font-weight: bold;
}

.grand-total {
  background-color: #f0f0f0;
  font-weight: bold;
}
</style>