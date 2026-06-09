<script setup>
import { ref, onMounted } from 'vue'

const patients = ref([])
const city = ref('')
const activeOnly = ref(false)

async function loadPatients() {

  const response =
    await fetch(
      `https://localhost:7082/api/patients?activeOnly=${activeOnly.value}`
    )

  patients.value =
    await response.json()
}

onMounted(async () => {
  await loadPatients()
})

async function searchPatients() {

  let url

  if (city.value.trim() === '') {

    url =
      `https://localhost:7082/api/patients?activeOnly=${activeOnly.value}`
  }
  else {

    url =
      `https://localhost:7082/api/patients/search?city=${city.value}&activeOnly=${activeOnly.value}`
  }

  const response =
    await fetch(url)

  patients.value =
    await response.json()
}

async function handleInput() {

  if (city.value.trim() === '') {
    await loadPatients()
  }
}

async function handleActiveOnlyChange() {

  if (city.value.trim() === '') {
    await loadPatients()
  }
  else {
    await searchPatients()
  }
}
</script>

<template>

  <div class="header">

    <h1>CareBridge Patients</h1>

    <div>
      <input
        type="text"
        v-model="city"
        @input="handleInput"
        placeholder="Enter city" />

      <button @click="searchPatients">
        Search
      </button>
    </div>

  </div>

  <div class="count-section">

    <h3>
      Total No. of Patients: {{ patients.length }}
    </h3>

    <label>
      <input
        type="checkbox"
        v-model="activeOnly"
        @change="handleActiveOnlyChange" />

      Active Only
    </label>

  </div>

  <table border="1">

    <tr>
      <th>Patient Id</th>
      <th>Full Name</th>
      <th>City</th>
      <th>Status</th>
    </tr>

    <tr
      v-for="p in patients"
      :key="p.patientId">

      <td>{{ p.patientId }}</td>
      <td>{{ p.fullName }}</td>
      <td>{{ p.city }}</td>
      <td>{{ p.isActive ? 'Active' : 'Inactive' }}</td>

    </tr>

  </table>

</template>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.count-section {
  display: flex;
  align-items: center;
  gap: 20px;
  margin-bottom: 20px;
}

</style>