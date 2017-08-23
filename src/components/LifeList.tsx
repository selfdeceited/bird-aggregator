import * as React from "react"
import axios from 'axios';

interface LifeListProps {}
interface LifeListState {
    lifeList: LifeListDto[]
}
interface LifeListDto {
    birdid: number,
    name: string,
    dateMet: string,
    location: string
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
        return (
        <div className="body">
            <ol>
            {
                this.state.lifeList.map(x => 
            <li key={x.birdid}><b>{x.name}</b> - first discovered at {new Date(Date.parse(x.dateMet)).toDateString()} near {x.location}</li>)
            }
            </ol>
        </div>);
    }
}