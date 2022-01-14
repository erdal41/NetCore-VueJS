<template>
    <div class="navbar-container main-menu-content mt-2">
        <app-navbar-horizontal-layout-brand />
        <horizontal-nav-menu-items :items="navMenuItems" />
    </div>
</template>

<script>
    import AppNavbarHorizontalLayoutBrand from '@core/layouts/components/app-navbar/AppNavbarHorizontalLayoutBrand.vue'
    import navMenuItems from '@/navigation/horizontal'
    import HorizontalNavMenuItems from './components/horizontal-nav-menu-items/HorizontalNavMenuItems.vue'


    export default {
        components: {
            AppNavbarHorizontalLayoutBrand,
            HorizontalNavMenuItems,
        },
        setup() {
            return {
                navMenuItems,
            }
        },
    }
</script>

<style lang="scss">
    @import "~@core/scss/base/core/menu/menu-types/horizontal-menu.scss";
</style>
