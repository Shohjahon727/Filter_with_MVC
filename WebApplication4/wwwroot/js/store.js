


const store = new Vuex.Store({
    state: {
        filters: {
            selectedManufacturers: [],
            selectedColors: [],
            minPrice: null,
            maxPrice: null,
            colors: '',
            manufacturers: ''
        },
        pager: {
            currentPage: 1,
            pageSize: 10,
            totalPages: 0,
            totalItems: 0
        },
        cars: []
    },
    getters: {
        getColors(state) {
            return state.filters.colors;
        },
        getManufacturers(state) {
            return state.filters.manufacturers;
        },
        getSelectedColors(state) {
            return state.filters.selectedColors;
        },
        getSelectedManufacturers(state) {
            return state.filters.selectedManufacturers;
        },
        getMinPrice(state) {
            return state.filters.minPrice;
        },
        getMaxPrice(state) {
            return state.filters.maxPrice;
        },
        getPager(state) {
            return state.pager;
        },
        getCars(state) {
            return state.cars;
        }
    },
    mutations: {
        setFilters(state, filters) {
            state.filters = filters;
        },
        setCars(state, cars) {
            state.cars = cars;
        },
        setPager(state, pager) {
            state.pager = pager;
        },
        setColors(state, colors) {
            state.filters.colors = colors;
        }
    },

    actions: {
        async applyFilters({ state, commit }) {
            try {
                let queryParams = new URLSearchParams();
                if (state.filters.selectedColors.length) {
                    queryParams.append("filterbycolor", state.filters.selectedColors.join(','));
                }

                if (state.filters.selectedManufacturers.length) {
                    queryParams.append("filterbymanufacturer", state.filters.selectedManufacturers.join(','));
                }

                if (state.filters.minPrice) {
                    queryParams.append("MinPrice", state.filters.minPrice);
                }

                if (state.filters.maxPrice) {
                    queryParams.append("MaxPrice", state.filters.maxPrice);
                }

                if (state.pager.currentPage) {
                    queryParams.append("page", state.pager.currentPage);
                }

                if (state.pager.pageSize) {
                    queryParams.append("pageSize", state.pager.pageSize);
                }

                window.history.pushState(null, '', '/CarWithComponents/?' + queryParams.toString());
                const response = await axios.get('/CarWithComponents/Filter?' + queryParams.toString());
                console.log(response.data.data);
                commit('setCars', response.data.data);
                commit('setPager', {
                    totalItems: response.data.totalItems,
                    totalPages: Math.ceil(response.data.totalItems / state.pager.pageSize),
                    currentPage: state.pager.currentPage,
                    pageSize: state.pager.pageSize
                });
            }
            catch (error) {
                console.error("Error fetching filtered cars :", error);
            }
        }
    },
})