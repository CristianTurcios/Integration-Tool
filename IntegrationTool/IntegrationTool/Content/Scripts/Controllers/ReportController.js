var ReportController = function ($scope, $http, $location, $window) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.value = "";
    $scope.startDate ="";
    $scope.endDate="";
    $scope.request = {};
    $scope.sendRequest = sendRequest;
    $scope.listIntegrationCategory = [];
    $scope.listIntegrationType = [];
    $scope.listOperationWebServices = [];
    $scope.listDatabaseParameter = [];
    $scope.Report =
    {
        name:'Excel'     
    };
   
    enableCard();
    getListIntegrationCategory();
    getListIntegrationType();
    getListOperationWebServices();
    getListDatabaseParameter();

   
    function getListIntegrationCategory() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Report/getListCategoryIntegration', data, config).success(function (resp)
        {
            if (resp.type !== 'danger') {             
                $scope.listIntegrationCategory = resp;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $scope.message = "Error: " + resp;
            $scope.typeMessage = "danger";
        });      
    }

    function getListIntegrationType()
    {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Report/getListIntegrationType', data, config).success(function (resp) {
            if (resp.type !== 'danger') {               
                $scope.listIntegrationType = resp;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $scope.message = "Error: " + resp;
            $scope.typeMessage = "danger";
        });
    }

    function getListOperationWebServices()
    {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Report/getListOperationWebServices', data, config).success(function (resp) {
            if (resp.type !== 'danger') {                
                $scope.listOperationWebServices = resp;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $scope.message = "Error: " + resp;
            $scope.typeMessage = "danger";
        });
    }

    function getListDatabaseParameter()
    {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Report/getListDatabaseParameter', data, config).success(function (resp) {
            if (resp.type !== 'danger') {             
                $scope.listDatabaseParameter = resp;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $scope.message = "Error: " + resp;
            $scope.typeMessage = "danger";
        });
    }

    function sendRequest(req) {

        $('#submit').button('loading');
        setTimeout(function ()
        {
            $('#submit').button('reset');
        }, 1000);

        $scope.request.start = $scope.startDate;
        $scope.request.end = $scope.endDate;
        $scope.request.value2 = $scope.value;
        $scope.request.Report = $scope.Report.name;

        var mime = setMimeForFiles();
            
        var config = {
            headers: {'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'},
            responseType: 'arraybuffer' 
        }

        var data = $.param(req);     
       
        $http.post('Report/getDocumentReport', data, config).success(function (resp) {
            var file = new Blob([resp], { type: mime });
            var fileURL = URL.createObjectURL(file);
            window.open(fileURL);
            $scope.request = {};
        }).error(function (resp) {
            $scope.message = "Error: " + resp;
            $scope.typeMessage = "danger";
            $window.scrollTo(0, 0);
        });
        
    }

    function setMimeForFiles()
    {
        var mime = "";
        if ($scope.request.Report == "Excel")
            mime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        else
            mime = "application/pdf";

        return mime;
    }
  
    function enableCard()
    {       
        //////////////

        (function ($)
        {
            //Author: Brady Sammons
            //URL: www.bradysammons.com
            /* -------------------------------------------------------- */
            /*	//set Global variables
            /* -------------------------------------------------------- */
            var cards = $(".card-drop"),
                toggler = cards.find(".toggle"),
                links = cards.find("ul>li>a"),
                li = links.parent('li'),
                count = links.length,
                width = 100;

            //set z-Index of drop Items
            li.each(function (i) {
                $(this).css("z-index", count - i); //invert the index values
            });

            //set top margins & widths of li elements
            function setClosed() {
                li.each(function (index) {
                    $(this).css("top", index * 4)
                           .css("width", (width - index * .5) + "%")
                           .css("margin-left", (index * .25) + "%")
                });
                li.addClass('closed');
                toggler.removeClass("active");
            }
            setClosed();

            /* -------------------------------------------------------- */
            /*	Toggler Click handler
            /* -------------------------------------------------------- */
            toggler.on("mousedown", function () {

                var $this = $(this); //cache $(this)
                //if the menu is active:
                if ($this.is(".active")) {
                    setClosed();
                } else {
                    //if the menu is un-active:
                    $this.addClass("active");
                    li.removeClass('closed');
                    //set top margins
                    li.each(function (index) {
                        $(this).css("top", 60 * (index + 1))
                               .css("width", "100%")
                               .css("margin-left", "0px");
                    });
                }
            });

            /* -------------------------------------------------------- */
            /*	Links Click handler
            /* -------------------------------------------------------- */
            links.on("click", function (e) {
                var $this = $(this);
                ExtractValue($this);

                label = $this.data("label");
                icon = $this.children("i").attr("class");

                li.removeClass('active');
                if ($this.parent("li").is("active")) {
                    $this.parent('li').removeClass("active");
                } else {
                    $this.parent("li").addClass("active");

                }
                toggler.children("span").text(label);
                toggler.children("i").removeClass().addClass(icon);
                setClosed();
                e.preventDefault;
            });

            

        })(jQuery);
        /////
    }

    function ExtractValue(Value) {
        var anchor = Value[0];       
        $scope.value = $(anchor).attr("value");
    }

    $(function () {
        //Date range as a button
        $('#daterange-btn').daterangepicker(
            {
                ranges: {
                    'Today': [moment(), moment()],
                    'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                    'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                    'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                    'This Month': [moment().startOf('month'), moment().endOf('month')],
                    'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                },
                startDate: moment().subtract(29, 'days'),
                endDate: moment()
            },
            function (start, end) {
                $('#daterange-btn span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
                $scope.startDate = start.format('MM D, YYYY');
                $scope.endDate = end.format('MM D, YYYY');
                //prueba(start.format('MM D, YYYY'), end.format('MM D, YYYY'));
            }
        );

    });      
}

ReportController.$inject = ['$scope', '$http', '$location', '$window'];