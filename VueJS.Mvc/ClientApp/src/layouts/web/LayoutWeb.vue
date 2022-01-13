<template>
    <layout-web>
        <router-view />
    </layout-web>
</template>

<script>
    import LayoutWeb from '@core/layouts/layout-web/LayoutWeb.vue'

    export default {
        components: {
            LayoutWeb
        },
    }
</script>
