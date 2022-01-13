<template>
    <div class="navbar-container d-flex content align-items-center">
        <horizontal-nav-menu />
    </div>
</template>

<script>
    import {
        BLink, BNavbarNav,
    } from 'bootstrap-vue'
    import Bookmarks from './components/Bookmarks.vue'
    import Locale from './components/Locale.vue'
    import DarkToggler from './components/DarkToggler.vue'
    import SearchBar from './components/SearchBar.vue'
    import CartDropdown from './components/CartDropdown.vue'
    import NotificationDropdown from './components/NotificationDropdown.vue'
    import UserDropdown from './components/UserDropdown.vue'
    import HorizontalNavMenu from './components/HorizontalNavMenu.vue'
    export default {
        components: {
            BLink,
            // Navbar Components
            HorizontalNavMenu,
            BNavbarNav,
            Bookmarks,
            Locale,
            DarkToggler,
            SearchBar,
            CartDropdown,
            NotificationDropdown,
            UserDropdown,
        },
        props: {
            toggleVerticalMenuActive: {
                type: Function,
                default: () => { },
            },
        },
    }
</script>
