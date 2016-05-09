function AddressSave() {
    if ($("#txtIP").val() != '') {
        $.ajax({
            type: "POST",
            url: './Demo/AddressAdd',
            processData: false,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ ipAddress: $("#txtIP").val(), hostName: $("#txtHost").val() }),
            success: function (result) {
                AddressSaveSuccess(result);
            },
            error: function (result) {
            }
        });
    }
}

function AddressRemove(ip, host) {
    $.ajax({
        type: "POST",
        url: './Demo/AddressRemove',
        processData: false,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ ipAddress: ip, hostName: host }),
        success: function (result) {
            AddressSaveSuccess(result);
        },
        error: function (result) {
        }
    });
}

function AddressPeek() {
    $.ajax({
        type: "POST",
        url: './Demo/AddressPeek',
        processData: false,
        contentType: "application/json; charset=utf-8",
        data:'',
        success: function (result) {
            AddressSaveSuccess(result);
        },
        error: function (result) {
        }
    });
}

function AddressTake() {
    $.ajax({
        type: "POST",
        url: './Demo/AddressTake',
        processData: false,
        contentType: "application/json; charset=utf-8",
        data: '',
        success: function (result) {
            AddressSaveSuccess(result);
        },
        error: function (result) {
        }
    });
}

function AddressGetAll() {
    $.ajax({
        type: "POST",
        url: './Demo/AddressGetAll',
        processData: false,
        contentType: "application/json; charset=utf-8",
        data: '',
        success: function (result) {
            AddressSaveSuccess(result);
        },
        error: function (result) {
        }
    });
}

function AddressSaveSuccess(result) {
    if (result.Success) {
        $("#dvCache").html(result.Message);
    }
}