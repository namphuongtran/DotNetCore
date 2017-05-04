StringUtil = {
    _unicodeChars: 'àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ',
    _nonUnicodeChars: 'aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIOOOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU',
    /**
    * Method: Convert Unicode characters to non-unicode characters
    */
    unicodeToNoneUnicode: function (s) {
        var retVal = '';
        if (!s || s === '')
            return retVal;
        var pos;
        for (var i = 0; i < s.length; i++) {
            pos = this._unicodeChars.indexOf(s[i].toString());
            if (pos >= 0)
                retVal += this._nonUnicodeChars[pos];
            else
                retVal += s[i];
        }
        return retVal;
    },

    removeSpecialCharacter: function (s) {
        s = s.replace(/[^\w\s]/gi, '')
        return s;
    },

    processString: function (s) {
        s = this.removeSpecialCharacter(s);
        s = s.replace(' ', '-');
        s = s.replace('--', '-');
        s = s.toLowerCase();
        return s;
    },

    buildUrl: function (param1, param2, param3) {
        var seoUrl = baseUri + param1 + '/' + this.processString(this.unicodeToNoneUnicode(param2)) + '.' + param3;
        return seoUrl;
    }
};
function City() {
    City.prototype.init.apply(this, arguments);
};

City.prototype = {
    init: function (containerId) {
        if (containerId == null || containerId.length <= 0) {
            typeof (console) != 'undefined' && console.log && cons.log('Not found `@containerId`');
            return;
        }

        this.$container = $(containerId);
        this.edit();
        this.onBlurValidate();
    },

    edit: function () {
        var _this = this;
        $('.btn-save', this.$container).off('click').on('click', function () {
            var lat = $('.lat-val', _this.$container).val(),
                lon = $('.lon-val', _this.$container).val(),
                cityId = parseInt($('.city-id', _this.$container).val()),
                city = $('.city-name', _this.$container).val(),
                url = baseUri + 'City/EditCity',
                $validationCity = $('.validation-city', _this.$container),
                $validationLat = $('.validation-lat', _this.$container),
                $validationLon = $('.validation-lon', _this.$container),
                $message = $('.message', _this.$container);

            //validate
            $message.empty();
            var isValid = _this.validate(cityId, city, lat, lon);
            if (isValid) {
                $validationCity.text('');
                $validationLat.text('');
                $validationLon.text('');

                var cityInfo = {
                    ID: cityId,
                    City: city,
                    Lat: parseFloat(lat),
                    Lon: parseFloat(lon)
                };
                $.ajax({
                    type: 'POST',
                    url: url,
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify(cityInfo),
                    success: function (result) {
                        if (result.isSuccess) {
                            $message.removeClass('text-danger').addClass('text-success');
                            $message.html(result.message);
                        }
                        else {
                            $message.html(result.message);
                        }
                    },
                    failure: function (errMsg) {
                        alert(errMsg);
                    }
                });
            } else {
                $message.html('An Error Has Occured');
            }
        });
    },

    validate: function (cityId, city, lat, lon) {
        var _this = this,
            $message = $('.message', _this.$container),
            $validationCity = $('.validation-city', _this.$container),
            $validationLat = $('.validation-lat', _this.$container),
            $validationLon = $('.validation-lon', _this.$container),
            isValid = true;

        if (isNaN(cityId) || cityId <= 0) {
            $message.html('This city does not existed.');
            isValid = false;
        }

        if (city == '') {
            $validationCity.text('City name is not empty');
            isValid = false;
        }

        if (lat == '') {
            $validationLat.text('Lat is incorrect');
            isValid = false;
        } else {
            if (!lat.match(/^[1-9]\d*(\.\d+)?$/)) {
                $validationLat.text('Lat is a number and only allow one dot');
                isValid = false;
            }
        }

        if (lon == '') {
            $validationLon.text('Lon is incorrect');
            isValid = false;
        } else {
            if (!lon.match(/^[1-9]\d*(\.\d+)?$/)) {
                $validationLon.text('Lat is a number and only allow one dot');
                isValid = false;
            }
        }

        return isValid;
    },

    onBlurValidate: function () {
        var _this = this,
            $lat = $('.lat-val', _this.$container),
            $lon = $('.lon-val', _this.$container),
            $validationLat = $('.validation-lat', _this.$container),
            $validationLon = $('.validation-lon', _this.$container);

        $lat.off('blur').on('blur', function () {
            var lat = $(this).val();
            if (lat == '') {
                $validationLat.text('Lat is incorrect');
            } else {
                if (!lat.match(/^[1-9]\d*(\.\d+)?$/)) {
                    $validationLat.text('Lat is a number and only allow one dot');
                } else {
                    $validationLat.text('');
                }
            }
        });

        $lon.off('blur').on('blur', function () {
            var lon = $(this).val();
            if (lon == '') {
                $validationLon.text('Lon is incorrect');
            } else {
                if (!lon.match(/^[1-9]\d*(\.\d+)?$/)) {
                    $validationLon.text('Lat is a number and only allow one dot');
                } else {
                    $validationLon.text('');
                }
            }
        });
    }
};
$(document).ready(function () {
    var cityObj = new City('.city-form');
    var $cityTable = $('.city-table');
    if ($cityTable.length > 0) {
        $cityTable.find('a.btn-cityname').off('click').on('click', function () {
            var cityName = $(this).html(),
                cityId = parseInt($(this).attr('city-id'));
            window.location.href = StringUtil.buildUrl('details', cityName, cityId)
        });
    }
});