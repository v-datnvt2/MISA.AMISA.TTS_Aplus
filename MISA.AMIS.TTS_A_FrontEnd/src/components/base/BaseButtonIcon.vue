<template>
  <button
    :class="['button btn-icon', subClass, iconName]"
    @click="$emit('btnClick')"
  >
    {{ buttonText }}
  </button>
</template>
<script>
export default {
  name: "ButtonIcon",
  props: {
    iconName: String,
    buttonText: String,
    subClass: String,
  },
};
</script>
