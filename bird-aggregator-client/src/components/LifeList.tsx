import * as Blueprint from "@blueprintjs/core"
import { Button, Icon, Popover } from "@blueprintjs/core"
import * as axios from "../http.adapter"
import moment from "moment"
import * as React from "react"
import { Link } from "react-router-dom"
import { MapWrap } from "./MapWrap"
import { YearlyLifeList } from "./YearlyLifeList"

interface ILifeListProps {}
interface ILifeListState {
    lifeList: ILifeListDto[],
}

interface ILifeListDto {
    birdId: number,
    name: string,
    dateMet: string,
    location: string,
    locationId: number,
    photoId: number
}

export class LifeList extends React.Component<ILifeListProps, ILifeListState> {
    constructor(props: ILifeListProps) {
        super(props)
        this.state = {
            lifeList: [],
        }
    }
    public componentDidMount() {
        axios.get(`/api/birds/lifelist`).then(res => {
            const lifeList = res.data as ILifeListDto[]
            this.setState({ lifeList })
        })
    }

    public render() {
        const popover = (x: ILifeListDto) => (x.locationId > 0) ?
            (<Blueprint.Popover
                target={<Blueprint.Button className="pt-button pt-minimal pt-icon-map-marker display-block"/>}
                content={<MapWrap asPopup={true} locationIdToShow={x.locationId}/>}/>) : <div></div>

        return (
<div className="body">
    <h2 className="show-mobile">Life lis t</h2>
    <div>
        <Popover>
        <Button>
            <Icon icon="calendar" />&nbsp;Yearly statistics
        </Button>
        <div>
            <YearlyLifeList/>
        </div>
        </Popover>
    </div>
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
                this.state.lifeList.map((x: ILifeListDto, i: number) =>
                (
                    <tr key={i}>
                        <td className="hide-mobile">{i + 1}</td>
                        <td className="bird-column">
                            {x.name}
                            <Link
                                key={x.photoId}
                                to={"/photos/" + x.photoId}
                                role="button"
                                className="pt-button pt-minimal pt-icon-arrow-right">
                            </Link>
                        </td>
                        <td className="date-column">{moment(x.dateMet).format("YYYY MM DD")}</td>
                        <td>
                            <span className="hide-mobile">{x.location}&nbsp;&nbsp;</span>
                            {popover(x)}
                        </td>
                    </tr>
                ))
            }
        </tbody>
    </table>
</div>)
    }
}
