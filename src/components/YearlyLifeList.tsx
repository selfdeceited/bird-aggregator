import axios from "axios"
import * as React from "react"

interface IYearlyLifeListProps {}
interface IYearlyLifeListState {
    lifeList: IYearlyLifeListDto[]
}

interface IYearlyLifeListDto {
    key: number,
    count: number
}

export class YearlyLifeList extends React.Component<IYearlyLifeListProps, IYearlyLifeListState> {
    constructor(props) {
        super(props)
        this.state = {
            lifeList: [],
        }
    }
    public componentDidMount() {
        axios.get(`/api/birds/lifelist/peryear`).then(res => {
            const lifeList = res.data as IYearlyLifeListDto[]
            this.setState({ lifeList })
        })
    }

    public render() {
        return (
<div>
    <table className="pt-table pt-striped">
        <thead>
            <tr>
                <th className="hide-mobile">Year</th>
                <th># of species met</th>
            </tr>
        </thead>
        <tbody className="life-list-table">
            {
                this.state.lifeList.map((x: IYearlyLifeListDto, i: number) =>
                (
                    <tr key={i}>
                        <td className="bird-column">
                            {x.key}
                        </td>
                        <td className="date-column">{x.count}</td>
                    </tr>
                ))
            }
        </tbody>
    </table>
</div>)
    }
}
