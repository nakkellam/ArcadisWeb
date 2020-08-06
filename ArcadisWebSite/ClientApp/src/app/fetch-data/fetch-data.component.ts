import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
    selector: 'app-fetch-data',
    templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
    public forecasts: Equipment[];
    public httpclnt: HttpClient;
    public baseUrl: string;
    public searchstr: string;
    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.httpclnt = http;
        this.baseUrl = baseUrl;
        
    }
    LoadData() {
        this.httpclnt.get<Equipment[]>(this.baseUrl + 'api/Equipments').subscribe(result => {
            this.forecasts = result;
        }, error => console.error(error));
    }
    editDomain(domain: any) {
        if (!domain.foraddition) {
            this.forecasts = this.forecasts.filter(item => item.foraddition != true);
            this.forecasts = this.forecasts.map(item => {

                // printing element 
                if (item.equipmentId == domain.equipmentId)
                    item.editable = true;
                else
                    item.editable = false;
                return item;
            });
        }
        else {

        }
        // domain.editable = !domain.editable;
    }
    cancel() {
        this.forecasts = this.forecasts.map(item => {

            // printing element 
            item.foraddition = false;
            item.editable = false;
            return item;
        });
    }
    search(domain: any) {
        this.httpclnt.get<Equipment[]>(this.baseUrl + 'api/Equipments/' + this.searchstr).subscribe(result => {
            this.forecasts = result;
        }, error => console.error(error));
        // domain.editable = !domain.editable;
    }
    addequipment(newEquipment: Equipment) {
        newEquipment = {} as Equipment;
        newEquipment.cost = 0;
        newEquipment.quantity = 0;
        newEquipment.foraddition = true;
        newEquipment.editable = true;
        if (this.forecasts) {
            this.forecasts = this.forecasts.map(item => {

                // printing element 
                item.foraddition = false;
                item.editable = false;
                return item;
            });
        }
        else {
            this.forecasts = [] as Equipment[];
        }
        this.forecasts.push(newEquipment)

    }
    export(domain: any) {



        this.httpclnt.get(this.baseUrl + 'api/Equipments/GetExcel/' + this.searchstr, { responseType: 'blob', observe: 'response' }).subscribe((result: any) => {
            try {
                var contentType = 'application/vnd.ms-excel';
                var blob = new Blob([result.body], { type: contentType });

                var downloadUrl = URL.createObjectURL(blob);
                var a = document.createElement("a");
                a.href = downloadUrl;
                a.download = "report";
                document.body.appendChild(a);
                a.click();

            } catch (exc) {

                console.log("Save Blob method failed with the following exception.");
                console.log(exc);
            }

        });
        // domain.editable = !domain.editable;
    }
    edit(domain: any) {
        if (!domain.foraddition) {
            this.httpclnt.put(this.baseUrl + 'api/Equipments/' + domain.equipmentId, domain).subscribe(result => {
                this.forecasts = this.forecasts.map(item => {

                    // printing element 

                    item.editable = false;
                    return item;
                });
                alert("Success!");
            }, error => {
                alert(JSON.stringify(error));
                console.error(error)
            });
        }
        else {
            this.httpclnt.post(this.baseUrl + 'api/Equipments', domain).subscribe(result => {
                this.forecasts = this.forecasts.map(item => {

                    // printing element 
                    item.foraddition = false;
                    item.editable = false;
                    return item;
                });
                alert("Success!");
            }, error => {
                alert(JSON.stringify(error));
                console.error(error)
            });
        }
    }
    deletedata(domain: any) {
        if (!domain.foraddition) {
            this.httpclnt.delete(this.baseUrl + 'api/Equipments/' + domain.equipmentId, domain).subscribe(result => {
                this.httpclnt.get<Equipment[]>(this.baseUrl + 'api/Equipments').subscribe(result => {
                    this.forecasts = result;
                }, error => console.error(error));
                alert("Success!");
            }, error => {
                alert(JSON.stringify(error));
                console.error(error)
            });
        }
        else {
            this.forecasts = this.forecasts.filter(item => item.foraddition != true);
        }

    }
}
interface Equipment {
    equipmentId: number;
    title: string;
    cost: number;
    quantity: number;
    totalcost: number;
    editable: boolean;
    foraddition: boolean;

}
