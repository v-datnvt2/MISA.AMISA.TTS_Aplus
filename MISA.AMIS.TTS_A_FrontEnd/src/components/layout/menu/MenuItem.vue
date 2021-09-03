<template>
     <div class="menu-item"  :class="[{'selected': isSelected}]" @click="menuItemOnClick()">
        <div class="menu-item-icon" :class="[icon, {'selected': isSelected}]"></div>
        <div class="menu-item-text" :class="{'minimized': contentExpanded}">{{menuItemText}}</div>
      </div>
</template>

<script>
export default {
    name: "MenuItem",
    props:{
        contentExpanded:Boolean,
        icon:String,
        menuItemText:String,
        // isSelected:Boolean,
    },
    data() {
        return {
            isSelected: false,
        }
    },
    methods:{
        menuItemOnClick(){
            this.isSelected= !this.isSelected;
            console.log("click menuitem")
        }
    }

}
</script>