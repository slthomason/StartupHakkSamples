<script setup lang="ts">
  import { ref } from 'vue'
  const cityName = ref('')
  const stateName = ref('')
  const createCity = async () => {
    const city = cityName.value
    const state = stateName.value
    const url = `http://127.0.0.1:8080/city/add/${state}/${city}`
    const response = await fetch(url, {
      method: "GET"
    });
    console.log(response);
  }
</script>

<template>
  <div>
    <input type="text" v-model="cityName" />
    <input type="text" v-model="stateName" />
    <button @click="createCity">Add</button>
  </div>
</template>