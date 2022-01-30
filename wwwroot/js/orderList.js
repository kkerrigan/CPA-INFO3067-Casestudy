var link = '/GetOrders';
var detailslink = '/GetOrderDetails/';
// register modal component
Vue.component('ordermodal', {
    template: '#modal-template',
    props: {
        item: {},
        modalItem: {},
        details: []
    }
});


new Vue({
    el: '#orders',
    methods: {
        GetOrders: function () {
            var self = this;
            axios.get(link).then(function (response) {
                self.orders = response.data;
            }, function (error) {
                console.log(error.statusText);
            });
        },
        loadAndShowModal: function () {
            var self = this;
            axios.get(detailslink + this.modalItem.id).then(function (response) {
                self.details = response.data;
                self.showModal = true;
            }, function (error) {
                console.log(error.statusText);
            });
        }
    },
    mounted: function () {
        this.GetOrders();
    },
    data: {
        orders: [],
        showModal: false,
        modalItem: {},
        details: []
    }
});

//
// Date formatter
// - obtained from the internet unknown source
//
function formatDate(date) {
    var d;
    if (date === undefined) {
        d = new Date(); //no date coming from server
    }
    else {
        d = new Date(Date.parse(date)); // date from server
    }
    var _day = d.getDate();
    var _month = d.getMonth() + 1;
    var _year = d.getFullYear();
    var _hour = d.getHours();
    var _min = d.getMinutes();
    if (_min < 10) { _min = "0" + _min; }
    return _year + "-" + _month + "-" + _day + " " + _hour + ":" + _min;
}

//
// Currency formatter
// - obtained from the internet unknown source
//
function cur(num) {
    num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' +
            num.substring(num.length - (4 * i + 3));
    return (((sign) ? '' : '-') + '$' + num + '.' + cents);
} //cur
