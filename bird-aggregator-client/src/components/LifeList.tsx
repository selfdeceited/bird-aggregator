import * as Blueprint from '@blueprintjs/core'
import { Button, Icon, Popover } from '@blueprintjs/core'
import * as axios from '../http.adapter'
import moment from 'moment'
import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { MapWrap } from './MapWrap'
import { YearlyLifeList } from './YearlyLifeList'

interface ILifeListDto {
	birdId: number
	name: string
	dateMet: string
	location: string
	locationId: number
	photoId: number
}

export const LifeList: React.FC = () => {
	const [lifelist, setLifelist] = useState<ILifeListDto[]>([])

	const fetchLifelist = async () => {
		const res = await axios.get('/api/lifelist')
		setLifelist(res.data.firstOccurences as ILifeListDto[])
	}

	useEffect(() => {
		fetchLifelist()
	}, [])

	const popover = (x: ILifeListDto) => (x.locationId > 0) ?
		(<Blueprint.Popover
			target={<Blueprint.Button className="bp3-button bp3-minimal bp3-icon-map-marker display-block"/>}
			content={<MapWrap asPopup={true} locationIdToShow={x.locationId}/>}/>) : <div></div>

	return (
		<article className="body lifelist-container">
			<h2 className="show-mobile">Life list</h2>
			<div>
				<Popover>
					<Button>
						<Icon icon="calendar" />&nbsp;&nbsp;&nbsp;Yearly statistics
					</Button>
					<div>
						<YearlyLifeList/>
					</div>
				</Popover>
			</div>
			<table className="bp3-table bp3-striped">
				<thead>
					<tr>
						<th className="hide-mobile">&nbsp;</th>
						<th>Species</th>
						<th>Date</th>
						<th><span className="hide-mobile">Location</span></th>
					</tr>
				</thead>
				<tbody className="life-list-table">
					{
						lifelist.map((x: ILifeListDto, i: number) =>
							(
								<tr key={i}>
									<td className="hide-mobile">{i + 1}</td>
									<td className="bird-column">
										{x.name}
										<Link
											key={x.photoId}
											to={'/photos/' + x.photoId}
											role="button"
											className="bp3-button bp3-minimal bp3-icon-arrow-right">
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
		</article>)
}
