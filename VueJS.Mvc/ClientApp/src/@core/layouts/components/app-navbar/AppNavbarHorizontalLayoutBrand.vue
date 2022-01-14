<template>
    <div class="navbar-header d-xl-block float-left">
        <ul class="nav navbar-nav p-2">
            <li class="nav-item">
                <b-link class="navbar-brand"
                        to="/">
                    <span class="brand-logo">
                        <b-img :src="appLogoImage"
                               alt="logo" 
                               height="50"/>
                    </span>
                </b-link>
            </li>
        </ul>
    </div>
</template>

<script>
    import { BLink, BImg } from 'bootstrap-vue'
    import { $themeConfig } from '@themeConfig'

    export default {
        components: {
            BLink,
            BImg,
        },
        setup() {
            // App Name
            const { appName, appLogoImage } = $themeConfig.app
            return {
                appName,
                appLogoImage,
            }
        },
    }
</script>

<style>
</style>
