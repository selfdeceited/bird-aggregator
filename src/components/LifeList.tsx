import * as React from "react"
import axios from 'axios';
import { Link } from 'react-router-dom'

interface LifeListProps {}
interface LifeListState {
    lifeList: LifeListDto[]
}

interface LifeListDto {
    birdId: number,
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
    <table className="pt-table pt-striped">
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
                this.state.lifeList.map((x: LifeListDto, i: number) => 
                (
                    <tr>
                        <td>{i + 1}</td>
                        <td className="bird-column">
                            {x.name}
                            <Link 
                                key={x.birdId}
                                to={"/birds/" + x.birdId}
                                role="button"
                                className="pt-button pt-minimal pt-icon-arrow-right">
                            </Link>
                        </td>
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