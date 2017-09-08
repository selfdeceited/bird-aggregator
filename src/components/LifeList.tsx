import * as React from "react"
import * as Blueprint from "@blueprintjs/core";
import axios from 'axios';
import { Link } from 'react-router-dom'
import { MapWrap } from './MapWrap'
import * as moment from 'moment';

interface LifeListProps {}
interface LifeListState {
    lifeList: LifeListDto[]
}

interface LifeListDto {
    birdId: number,
    name: string,
    dateMet: string,
    location: string,
    locationId: number
}

export class LifeList extends React.Component<LifeListProps, LifeListState> {
    constructor(props) {
        super(props);
        this.state = {
            lifeList: []
        };
    }
    componentDidMount() {
        axios.get(`/api/birds/lifelist`).then(res => {
            const lifeList = res.data as LifeListDto[];
            this.setState({ lifeList });
        });
    }
    
    render() {
        const popover = x => (x.locationId > 0) ?
            (<Blueprint.Popover
                target={<Blueprint.Button className="pt-button pt-minimal pt-icon-map-marker display-block"/>}
                content={<MapWrap asPopup={true} locationIdToShow={x.locationId}/>}/>): <div></div>


        return (
<div className="body">
    <h2 className="show-mobile">Life list</h2>
    <table className="pt-table pt-striped">
        <thead>
            <tr>
                <th className="hide-mobile">#</th>
                <th>Species</th>
                <th>Date</th>
                <th><span className="hide-mobile">Location</span></th>
            </tr>
        </thead>
        <tbody className="life-list-table">
            {
                this.state.lifeList.map((x: LifeListDto, i: number) => 
                (
                    <tr>
                        <td className="hide-mobile">{i + 1}</td>
                        <td className="bird-column">
                            {x.name}
                            <Link 
                                key={x.birdId}
                                to={"/birds/" + x.birdId}
                                role="button"
                                className="pt-button pt-minimal pt-icon-arrow-right">
                            </Link>
                        </td>
                        <td className="date-column">{moment(x.dateMet).format('YYYY MM DD')}</td>
                        <td>
                            <span className="hide-mobile">{x.location}&nbsp;&nbsp;</span>
                            {popover(x)}
                        </td>
                    </tr>
                ))
            }
        </tbody>
    </table>
</div>);
    }
}