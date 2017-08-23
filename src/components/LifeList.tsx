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
    <table className="pt-table pt-striped pt-interactive">
        <thead>
            <tr>
                <th>#</th>
                <th>Species</th>
                <th>Date</th>
                <th>Location</th>
            </tr>
        </thead>
        <tbody>
            {
                this.state.lifeList.map((x, i) => 
                (
                    <tr>
                        <td>{i + 1}</td>
                        <td>{x.name}</td>
                        <td>{new Date(Date.parse(x.dateMet)).toDateString()}</td>
                        <td>{x.location}</td>
                    </tr>
                ))
            }
        </tbody>
    </table>
</div>);
    }
}