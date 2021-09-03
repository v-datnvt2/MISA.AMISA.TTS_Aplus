<template>
  <router-view></router-view>
</template>
<script>
// import Header from "./TheHeader.vue";
export default {
  name: "Content",
  props: {},

  data() {
    return {
      showForm: false,
    };
  },
  components: {
    // Header,
  },
  methods: {},
};
</script>
