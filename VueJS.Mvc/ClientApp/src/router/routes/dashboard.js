export default [
    {
        path: '/admin/dashboard',
        name: 'dashboard-analytics',
        component: () => import('@/views/dashboard/analytics/Analytics.vue'),
        meta: {
            resource: 'Dashboard',
            action: 'read',
        },
    },
    {
        path: '/admin/dashboard/ecommerce',
        name: 'dashboard-ecommerce',
        component: () => import('@/views/dashboard/ecommerce/Ecommerce.vue'),
        meta: {
            resource: 'Dashboard',
            action: 'read',
        },
    },
]