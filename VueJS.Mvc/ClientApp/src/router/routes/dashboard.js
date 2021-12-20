export default [
    {
        path: '/admin/dashboard/analytics',
        name: 'dashboard-analytics',
        component: () => import('@/views/admin/dashboard/analytics/Analytics.vue'),
    },
    {
        path: '/admin/dashboard/ecommerce',
        name: 'dashboard-ecommerce',
        component: () => import('@/views/admin/dashboard/ecommerce/Ecommerce.vue'),
    },
]